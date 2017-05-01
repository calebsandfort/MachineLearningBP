using MachineLearningBP.CollectiveIntelligence.DomainServices.Examples;
using MachineLearningBP.Entities.Sports.Nba;
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

namespace MachineLearningBP.Services.Sports.Nba
{
    public class NbaPointsExampleDomainService : MaximumExampleDomainService<NbaGame, NbaStatLine, NbaPointsExample, Double, NbaExampleGenerationInfo, NbaSeason, NbaTeam>, INbaPointsExampleDomainService
    {
        #region Properties
        public readonly IKNearestNeighborsDomainService<NbaPointsExample, NbaStatLine, Double> _kNearestNeighborsDomainService;
        private readonly ISheetUtilityDomainService _sheetUtilityDomainService;
        public readonly ICommandRunner _commandRunner;
        #endregion

        #region Constructor
        public NbaPointsExampleDomainService(IRepository<NbaGame> sampleRepository, IRepository<NbaStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager,
            IRepository<NbaPointsExample> exampleRepository, IRepository<NbaSeason> timeGroupingRepository, ISheetUtilityDomainService sheetUtilityDomainService,
            IRepository<NbaTeam> participantRepository, IKNearestNeighborsDomainService<NbaPointsExample, NbaStatLine, Double> kNearestNeighborsDomainService,
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
                    List<NbaSeason> seasons;
                    List<NbaTeam> teams;
                    List<NbaGame> games, todaysGames;

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        currentDate = this._sampleRepository.GetAll().Where(x => x.Completed).OrderBy(x => x.Date).First().Date.AddDays(-1);
                        seasons = this._timeGroupingRepository.GetAll().OrderBy(x => x.Start).ToList();
                        teams = await this._participantRepository.GetAllListAsync();
                        await unitOfWork.CompleteAsync();
                    }

                    foreach (NbaSeason season in seasons.OrderBy(x => x.Start))
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
        public async Task PopulateExample(NbaGame game, List<NbaGame> games, List<NbaTeam> teams, int rollingWindowPeriod, double scaleFactor)
        {
            try
            {
                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    NbaExampleGenerationInfo awayInfo = new NbaExampleGenerationInfo(game, games, teams, false, rollingWindowPeriod, scaleFactor);
                    NbaPointsExample awayExample = new NbaPointsExample();
                    awayExample.SetFields(awayInfo.TeamStatLine1, awayInfo);
                    await this._exampleRepository.InsertAsync(awayExample);

                    NbaExampleGenerationInfo homeInfo = new NbaExampleGenerationInfo(game, games, teams, true, rollingWindowPeriod, scaleFactor);
                    NbaPointsExample homeExample = new NbaPointsExample();
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
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting NbaPointsExamples..."));

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute($"DELETE FROM [NbaPointsExamples]");
                unitOfWork.Complete();
            }

            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Deleting NbaPointsExamples finished."));
        }
        #endregion

        #region KNearestNeighborsDoStuff
        public async Task KNearestNeighborsDoStuff()
        {
            KNearestNeighborsCrossValidateInput<NbaPointsExample, NbaStatLine, Double> input = new KNearestNeighborsCrossValidateInput<NbaPointsExample, NbaStatLine, double>();
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
            await _backgroundJobManager.EnqueueAsync<NbaPointsFindOptimalParametersBackgroundJob, bool>(record);
        }

        public async Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParameters(bool record)
        {
            NbaPointsExample[] data = await this.GetExamples();

            List<KNearestNeighborsCrossValidateResult> results = this._kNearestNeighborsDomainService.FindOptimalParameters(data);

            if (record && results.Count > 0)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>(results));

            return results;
        }
        #endregion

        #region FindOptimalParametersPython
        public async Task FindOptimalParametersPythonEnqueue(bool record)
        {
            await _backgroundJobManager.EnqueueAsync<NbaPointsFindOptimalParametersPythonBackgroundJob, bool>(record);
        }

        public async Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParametersPython(bool record)
        {
            NbaPointsExample[] data = await this.GetExamples();

            List<KNearestNeighborsCrossValidateResult> results = this._kNearestNeighborsDomainService.FindOptimalParametersPython(data);

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
            await _backgroundJobManager.EnqueueAsync<NbaPointsAnnealingOptimizeBackgroundJob, AnnealingOptimizeInput>(input);
        }

        public async Task AnnealingOptimize(AnnealingOptimizeInput input)
        {
            NbaPointsExample[] data = await this.GetExamples();
            OptimizeResult result = this._kNearestNeighborsDomainService.AnnealingOptimize(input, data);

            if (input.record)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>() { result });
        }
        #endregion

        #region GeneticOptimize
        public async Task GeneticOptimizeEnqueue(GeneticOptimizeInput input)
        {
            await _backgroundJobManager.EnqueueAsync<NbaPointsGeneticOptimizeBackgroundJob, GeneticOptimizeInput>(input);
        }

        public async Task GeneticOptimize(GeneticOptimizeInput input)
        {
            NbaPointsExample[] data = await this.GetExamples();
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
                    NbaPointsExample[] data = await this.GetExamples();
                    this._kNearestNeighborsDomainService.WritePythonDataFile(data);
                    //public double[] WeightedKnn(TExample[] data, TExample v1, Func<Double, Double> weightf, int[] ks)
                    Func<Double, Double> weightf = (d) => this._kNearestNeighborsDomainService.InverseWeight(d);

                    KNearestNeighborsCrossValidateInput<NbaPointsExample, NbaStatLine, Double> input = new KNearestNeighborsCrossValidateInput<NbaPointsExample, NbaStatLine, double>();
                    input.GuessMethod = KNearestNeighborsGuessMethods.WeightedKnn;
                    input.WeightMethod = KNearestNeighborsWeightMethods.InverseWeight;
                    input.Ks = new int[] { 40 };
                    

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        DateTime now = Clock.Now;
                        List<NbaPointsExample> todaysExamples = this._exampleRepository.GetAll().Where(x => x.Date.Year == now.Year && x.Date.Month == now.Month && x.Date.Day == now.Day).ToList();

                        foreach (NbaPointsExample example in todaysExamples)
                        {
                            NbaStatLine statLine = await this._statLineRepository.GetAsync(example.StatLineId);
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
        public async Task<NbaPointsExample[]> GetExamples()
        {
            NbaPointsExample[] data;

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                //data = this._exampleRepository.GetAll().Where(x => x.Date < Clock.Now.Date).ToArray();
                data = this._exampleRepository.GetAll().Where(x => x.Date < Clock.Now.Date).OrderByDescending(x => x.StatLine.Sample.Date).Take(500).ToArray();
                unitOfWork.Complete();
            }

            return data;
        }
        #endregion
    }
}
