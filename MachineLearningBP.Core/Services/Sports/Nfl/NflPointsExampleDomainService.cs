using MachineLearningBP.CollectiveIntelligence.DomainServices.Examples;
using MachineLearningBP.Entities.Sports.Nfl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Domain.Repositories;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Shared.GuerillaTimer;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms;
using MachineLearningBP.Core.Services;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using Abp.BackgroundJobs;
using MachineLearningBP.BackgroundJobs.Sports;
using Abp.Timing;
using MachineLearningBP.Shared.CommandRunner;

namespace MachineLearningBP.Services.Sports.Nfl
{
    public class NflPointsExampleDomainService : MaximumExampleDomainService<NflGame, NflStatLine, NflPointsExample, Double, NflExampleGenerationInfo, NflSeason, NflTeam>, INflPointsExampleDomainService
    {
        #region Properties
        public readonly IKNearestNeighborsDomainService<NflPointsExample, NflStatLine, Double> _kNearestNeighborsDomainService;
        private readonly ISheetUtilityDomainService _sheetUtilityDomainService;
        public readonly ICommandRunner _commandRunner;
        #endregion

        #region Constructor
        public NflPointsExampleDomainService(IRepository<NflGame> sampleRepository, IRepository<NflStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager,
            IRepository<NflPointsExample> exampleRepository, IRepository<NflSeason> timeGroupingRepository, ISheetUtilityDomainService sheetUtilityDomainService,
            IRepository<NflTeam> participantRepository, IKNearestNeighborsDomainService<NflPointsExample, NflStatLine, Double> kNearestNeighborsDomainService,
            IBackgroundJobManager backgroundJobManager, ICommandRunner commandRunner)
            : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy,
                settingManager, exampleRepository, timeGroupingRepository, participantRepository, backgroundJobManager)
        {
            this._kNearestNeighborsDomainService = kNearestNeighborsDomainService;
            this._sheetUtilityDomainService = sheetUtilityDomainService;
            this._commandRunner = commandRunner;
        }
        #endregion

        #region PopulateExamples
        public async Task PopulateExamples(int rollingWindowPeriod, double scaleFactor)
        {
            try
            {
                using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
                {
                    this.DeleteExamples();

                    DateTime now = DateTime.Now.Date;
                    DateTime currentDate;
                    List<NflSeason> seasons;
                    List<NflTeam> teams;
                    List<NflGame> games, todaysGames;

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        currentDate = this._sampleRepository.GetAll().Where(x => x.Completed).OrderBy(x => x.Date).First().Date.AddDays(-1);
                        seasons = this._timeGroupingRepository.GetAll().OrderBy(x => x.Start).ToList();
                        teams = await this._participantRepository.GetAllListAsync();
                        await unitOfWork.CompleteAsync();
                    }

                    foreach (NflSeason season in seasons.OrderBy(x => x.Start))
                    {
                        this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Starting {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season..."));

                        using (var unitOfWork = this.UnitOfWorkManager.Begin())
                        {
                            games = this._sampleRepository.GetAllIncluding(x => x.StatLines)
                                .Where(x => x.TimeGroupingId == season.Id).ToList();
                            await unitOfWork.CompleteAsync();
                        }

                        if (currentDate < season.Start.Date) currentDate = season.RollingWindowStart.Value.Date;

                        while (season.Within(currentDate))
                        {
                            if (currentDate > now) break;

                            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Populating {currentDate.ToShortDateString()} ..."));

                            using (var unitOfWork = this.UnitOfWorkManager.Begin())
                            {
                                todaysGames = this._sampleRepository.GetAllIncluding(x => x.StatLines).Where(x => x.Date.Year == currentDate.Year && x.Date.Month == currentDate.Month && x.Date.Day == currentDate.Day).ToList();
                                await unitOfWork.CompleteAsync();
                            }

                            await Task.WhenAll(todaysGames.Select(x => PopulateExample(x, games, teams, rollingWindowPeriod, scaleFactor)).ToArray());
                            currentDate = currentDate.AddDays(1);
                        }

                        this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Completed {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season."));
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }
        #endregion

        #region PopulateExample
        public async Task PopulateExample(NflGame game, List<NflGame> games, List<NflTeam> teams, int rollingWindowPeriod, double scaleFactor)
        {
            try
            {
                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    NflExampleGenerationInfo awayInfo = new NflExampleGenerationInfo(game, games, teams, false, rollingWindowPeriod, scaleFactor);
                    NflPointsExample awayExample = new NflPointsExample();
                    awayExample.SetFields(awayInfo.TeamStatLine1, awayInfo);
                    await this._exampleRepository.InsertAsync(awayExample);

                    NflExampleGenerationInfo homeInfo = new NflExampleGenerationInfo(game, games, teams, true, rollingWindowPeriod, scaleFactor);
                    NflPointsExample homeExample = new NflPointsExample();
                    homeExample.SetFields(homeInfo.TeamStatLine1, homeInfo);
                    await this._exampleRepository.InsertAsync(homeExample);

                    await unitOfWork.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }
        #endregion

        #region DeleteExamples
        public void DeleteExamples()
        {
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting NflPointsExamples..."));

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute($"DELETE FROM [NflPointsExamples]");
                unitOfWork.Complete();
            }

            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Deleting NflPointsExamples finished."));
        }
        #endregion

        #region KNearestNeighborsDoStuff
        public async Task KNearestNeighborsDoStuff()
        {
            KNearestNeighborsCrossValidateInput<NflPointsExample, NflStatLine, Double> input = new KNearestNeighborsCrossValidateInput<NflPointsExample, NflStatLine, double>();
            input.GuessMethod = KNearestNeighborsGuessMethods.WeightedKnn;
            input.WeightMethod = KNearestNeighborsWeightMethods.InverseWeight;
            input.Trials = 5;

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                input.Data = this._exampleRepository.GetAll().Take(500).ToArray();
                unitOfWork.Complete();
            }

            //KNearestNeighborsCrossValidateResult result = this._kNearestNeighborsDomainService.CrossValidate(input);
        }
        #endregion

        #region FindOptimalParameters
        public async Task FindOptimalParametersEnqueue(bool record)
        {
            //await _backgroundJobManager.EnqueueAsync<NflPointsFindOptimalParametersBackgroundJob, bool>(record);
        }

        public async Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParameters(bool record)
        {
            NflPointsExample[] data = await this.GetExamples();

            KNearestNeighborsOptimalParametersInput<NflPointsExample, NflStatLine, Double> optimalParametersInput = new KNearestNeighborsOptimalParametersInput<NflPointsExample, NflStatLine, double>();
            optimalParametersInput.Data = data;
            optimalParametersInput.DistanceMethod = KNearestNeighborsDistanceMethods.Euclidean;
            optimalParametersInput.GuessMethods.Add(KNearestNeighborsGuessMethods.KnnEstimate);
            optimalParametersInput.GuessMethods.Add(KNearestNeighborsGuessMethods.WeightedKnn);

            optimalParametersInput.WeightMethods.Add(KNearestNeighborsWeightMethods.Gaussian);
            optimalParametersInput.WeightMethods.Add(KNearestNeighborsWeightMethods.InverseWeight);
            optimalParametersInput.WeightMethods.Add(KNearestNeighborsWeightMethods.SubtractWeight);

            optimalParametersInput.Ks = new int[] { 10, 15, 20, 25, 30, 35, 40, 45, 50 };
            optimalParametersInput.SubtractWeightConstant = 30;
            optimalParametersInput.Trials = 25;

            List<KNearestNeighborsCrossValidateResult> results = this._kNearestNeighborsDomainService.FindOptimalParameters(optimalParametersInput);

            if (record && results.Count > 0)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>(results));

            return results;
        }
        #endregion

        #region FindOptimalParametersPython
        public async Task FindOptimalParametersPythonEnqueue(bool record)
        {
            //await _backgroundJobManager.EnqueueAsync<NflPointsFindOptimalParametersPythonBackgroundJob, bool>(record);
        }

        public async Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParametersPython(bool record)
        {
            NflPointsExample[] data = await this.GetExamples();

            KNearestNeighborsOptimalParametersInput<NflPointsExample, NflStatLine, Double> optimalParametersInput = new KNearestNeighborsOptimalParametersInput<NflPointsExample, NflStatLine, double>();
            optimalParametersInput.Data = data;
            optimalParametersInput.DistanceMethod = KNearestNeighborsDistanceMethods.Gower;
            optimalParametersInput.GuessMethods.Add(KNearestNeighborsGuessMethods.KnnEstimate);
            optimalParametersInput.GuessMethods.Add(KNearestNeighborsGuessMethods.WeightedKnn);

            optimalParametersInput.WeightMethods.Add(KNearestNeighborsWeightMethods.Gaussian);
            optimalParametersInput.WeightMethods.Add(KNearestNeighborsWeightMethods.InverseWeight);
            optimalParametersInput.WeightMethods.Add(KNearestNeighborsWeightMethods.SubtractWeight);

            optimalParametersInput.Ks = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            optimalParametersInput.SubtractWeightConstant = 1;
            optimalParametersInput.Trials = 25;

            List<KNearestNeighborsCrossValidateResult> results = this._kNearestNeighborsDomainService.FindOptimalParametersPythonAndR(optimalParametersInput);

            if (record && results.Count > 0)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>(results));

            return results;
        }
        #endregion

        #region BuildDecisionTree
        public async Task<DecisionNode> BuildDecisionTree()
        {
            DecisionNode decisionTree = new DecisionNode();
            return decisionTree;
        } 
        #endregion

        #region AnnealingOptimize
        public async Task AnnealingOptimizeEnqueue(AnnealingOptimizeInput input)
        {
            //await _backgroundJobManager.EnqueueAsync<NflPointsAnnealingOptimizeBackgroundJob, AnnealingOptimizeInput>(input);
        }

        public async Task AnnealingOptimize(AnnealingOptimizeInput input)
        {
            NflPointsExample[] data = await this.GetExamples();
            OptimizeResult result = this._kNearestNeighborsDomainService.AnnealingOptimize(input, data);

            if (input.record)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>() { result });
        }
        #endregion

        #region GeneticOptimize
        public async Task GeneticOptimizeEnqueue(GeneticOptimizeInput input)
        {
            //await _backgroundJobManager.EnqueueAsync<NflPointsGeneticOptimizeBackgroundJob, GeneticOptimizeInput>(input);
        }

        public async Task GeneticOptimize(GeneticOptimizeInput input)
        {
            NflPointsExample[] data = await this.GetExamples();
            OptimizeResult result = this._kNearestNeighborsDomainService.GeneticOptimize(input, data);

            if (input.record)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>() { result });
        } 
        #endregion

        #region PredictToday
        public async Task PredictToday()
        {
            try
            {
                using (GuerillaTimer timer = new GuerillaTimer(this._consoleHubProxy))
                {
                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        KNearestNeighborsOptimalParametersInput<NflPointsExample, NflStatLine, Double> optimalParametersInput = new KNearestNeighborsOptimalParametersInput<NflPointsExample, NflStatLine, double>();
                        List<NflPointsExample> data = new List<NflPointsExample>();
                        data.AddRange(await this.GetExamples());

                        DateTime now = Clock.Now;
                        List<NflPointsExample> todaysExamples = this._exampleRepository.GetAll().Where(x => x.Date.Year == now.Year && x.Date.Month == now.Month && x.Date.Day == now.Day).ToList();
                        //data.AddRange(todaysExamples);

                        optimalParametersInput.Data = data.ToArray();
                        optimalParametersInput.DistanceMethod = KNearestNeighborsDistanceMethods.Euclidean;

                        //Double[] daisyDistances = this._kNearestNeighborsDomainService.GetDaisyDistances(optimalParametersInput);
                        this._kNearestNeighborsDomainService.WritePythonDataFile(optimalParametersInput.Data);
                        //public double[] WeightedKnn(TExample[] data, TExample v1, Func<Double, Double> weightf, int[] ks)
                        Func<Double, Double> weightf = (d) => this._kNearestNeighborsDomainService.InverseWeight(d);

                        KNearestNeighborsCrossValidateInput<NflPointsExample, NflStatLine, Double> input = new KNearestNeighborsCrossValidateInput<NflPointsExample, NflStatLine, double>();
                        input.GuessMethod = KNearestNeighborsGuessMethods.WeightedKnn;
                        input.WeightMethod = KNearestNeighborsWeightMethods.InverseWeight;
                        input.Ks = new int[] { 40 };

                        foreach (NflPointsExample example in todaysExamples)
                        {
                            NflStatLine statLine = await this._statLineRepository.GetAsync(example.StatLineId);
                            example.Index = data.IndexOf(example);
                            input.Observation = example;
                            statLine.KnnPoints = input.GetPythonResults(this._commandRunner).First().Result;
                        }

                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }
        #endregion

        #region GetExamples
        public async Task<NflPointsExample[]> GetExamples()
        {
            NflPointsExample[] data;

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                data = this._exampleRepository.GetAll().Where(x => x.Date < Clock.Now.Date).ToArray();
                //data = this._exampleRepository.GetAll().Where(x => x.Date < Clock.Now.Date).OrderByDescending(x => x.StatLine.Sample.Date).Take(500).ToArray();
                unitOfWork.Complete();
            }

            return data;
        }
        #endregion
    }
}
