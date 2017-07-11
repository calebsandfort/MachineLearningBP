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
using MachineLearningBP.Entities;
using MachineLearningBP.Enums;
using Newtonsoft.Json;

namespace MachineLearningBP.Services.Sports.Nba
{
    public class NbaAtsTreeExampleDomainService : MaximumExampleDomainService<NbaGame, NbaStatLine, NbaAtsTreeExample, Double, NbaExampleGenerationInfo, NbaSeason, NbaTeam>, INbaAtsTreeExampleDomainService
    {
        #region Properties
        public readonly IKNearestNeighborsDomainService<NbaAtsTreeExample, NbaStatLine, Double> _kNearestNeighborsDomainService;
        public readonly IDecisionTreeDomainService<NbaAtsTreeExample, NbaStatLine, Double> _decisionTreeDomainService;
        private readonly ISheetUtilityDomainService _sheetUtilityDomainService;
        readonly IRepository<DecisionTree> _decisionTreeRepository;
        #endregion

        #region Constructor
        public NbaAtsTreeExampleDomainService(IRepository<NbaGame> sampleRepository, IRepository<NbaStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager,
            IRepository<NbaAtsTreeExample> exampleRepository, IRepository<NbaSeason> timeGroupingRepository, ISheetUtilityDomainService sheetUtilityDomainService,
            IRepository<NbaTeam> participantRepository, IKNearestNeighborsDomainService<NbaAtsTreeExample, NbaStatLine, Double> kNearestNeighborsDomainService,
            IBackgroundJobManager backgroundJobManager, IDecisionTreeDomainService<NbaAtsTreeExample, NbaStatLine, Double> decisionTreeDomainService,
            IRepository<DecisionTree> decisionTreeRepository)
            : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy,
                settingManager, exampleRepository, timeGroupingRepository, participantRepository, backgroundJobManager)
        {
            this._kNearestNeighborsDomainService = kNearestNeighborsDomainService;
            this._decisionTreeDomainService = decisionTreeDomainService;
            this._sheetUtilityDomainService = sheetUtilityDomainService;
            this._decisionTreeRepository = decisionTreeRepository;
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

                    #region First run-thru
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
                    #endregion

                    #region Standard Deviation
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Starting standard deviation calculations..."));
                    List<NbaAtsTreeExample> examples;
                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        examples = await this._exampleRepository.GetAllListAsync();
                        List<StandardDeviationCalculator> standardDeviations = new List<StandardDeviationCalculator>();
                        for(int i = 0; i < examples[0].OrdinalData.Count; i++)
                        {
                            standardDeviations.Add(new StandardDeviationCalculator(examples.Select(x => x.OrdinalData[i])));
                        }

                        foreach(NbaAtsTreeExample example in examples)
                        {
                            example.DelimitedOrdinalData = String.Join(":", example.OrdinalData.Select((x, idx) => standardDeviations[idx].CalculateZScore(x).ToString("N1")));
                        }

                        await unitOfWork.CompleteAsync();
                        this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Completed standard deviation calculations."));
                    }
                    #endregion
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
                    NbaAtsTreeExample awayExample = new NbaAtsTreeExample();
                    awayExample.SetFields(awayInfo.TeamStatLine1, awayInfo);
                    await this._exampleRepository.InsertAsync(awayExample);

                    NbaExampleGenerationInfo homeInfo = new NbaExampleGenerationInfo(game, games, teams, true, rollingWindowPeriod, scaleFactor);
                    NbaAtsTreeExample homeExample = new NbaAtsTreeExample();
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
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting NbaAtsTreeExamples..."));

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute($"DELETE FROM [NbaAtsTreeExamples]");
                unitOfWork.Complete();
            }

            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Deleting NbaAtsTreeExamples finished."));
        }
        #endregion

        #region KNearestNeighborsDoStuff
        public async Task KNearestNeighborsDoStuff()
        {
            KNearestNeighborsCrossValidateInput<NbaAtsTreeExample, NbaStatLine, Double> input = new KNearestNeighborsCrossValidateInput<NbaAtsTreeExample, NbaStatLine, Double>();
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

        #region FindOptimalParametersPython
        public Task FindOptimalParametersPythonEnqueue(bool record)
        {
            throw new NotImplementedException();
        }

        public Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParametersPython(bool record)
        {
            throw new NotImplementedException();
        } 
        #endregion

        #region FindOptimalParameters
        public async Task FindOptimalParametersEnqueue(bool record)
        {
            //await _backgroundJobManager.EnqueueAsync<NbaAtsTreeFindOptimalParametersBackgroundJob, bool>(record);
        }

        public async Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParameters(bool record)
        {
            NbaAtsTreeExample[] data = await this.GetExamples();

            KNearestNeighborsOptimalParametersInput<NbaAtsTreeExample, NbaStatLine, Double> optimalParametersInput = new KNearestNeighborsOptimalParametersInput<NbaAtsTreeExample, NbaStatLine, double>();
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

            List<KNearestNeighborsCrossValidateResult> results = this._kNearestNeighborsDomainService.FindOptimalParametersPythonAndR(optimalParametersInput);

            if (record)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>(results));

            return results;
        }
        #endregion

        #region BuildDecisionTree
        public async Task<DecisionNode> BuildDecisionTree()
        {
            using (GuerillaTimer timer = new GuerillaTimer(this._consoleHubProxy))
            {
                List<NbaAtsTreeExample> data = (await this.GetExamples()).ToList();
                DecisionNode root = this._decisionTreeDomainService.BuildTree(data, this._decisionTreeDomainService.Variance);

                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Pruning {JsonConvert.SerializeObject(root).Length}..."));
                this._decisionTreeDomainService.Prune(root, this._decisionTreeDomainService.Variance);
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Pruned {JsonConvert.SerializeObject(root).Length}."));

                try
                {
                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        DecisionTree decisionTree = await this._decisionTreeRepository.FirstOrDefaultAsync(x => x.Target == DecisionTreeTargets.NbaAts);
                        if (decisionTree == null)
                        {
                            decisionTree = new DecisionTree { Target = DecisionTreeTargets.NbaAts };
                            await this._decisionTreeRepository.InsertAsync(decisionTree);
                        }

                        decisionTree.Root = root;

                        unitOfWork.Complete();
                    }
                }
                catch (Exception ex)
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                    throw ex;
                }

                return root;
            }
        }
        #endregion

        #region AnnealingOptimize
        public async Task AnnealingOptimizeEnqueue(AnnealingOptimizeInput input)
        {
            //await _backgroundJobManager.EnqueueAsync<NbaAtsTreeAnnealingOptimizeBackgroundJob, AnnealingOptimizeInput>(input);
        }

        public async Task AnnealingOptimize(AnnealingOptimizeInput input)
        {
            NbaAtsTreeExample[] data = await this.GetExamples();
            OptimizeResult result = this._kNearestNeighborsDomainService.AnnealingOptimize(input, data);

            if (input.record)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>() { result });
        }
        #endregion

        #region GeneticOptimize
        public async Task GeneticOptimizeEnqueue(GeneticOptimizeInput input)
        {
            //await _backgroundJobManager.EnqueueAsync<NbaAtsTreeGeneticOptimizeBackgroundJob, GeneticOptimizeInput>(input);
        }

        public async Task GeneticOptimize(GeneticOptimizeInput input)
        {
            NbaAtsTreeExample[] data = await this.GetExamples();
            OptimizeResult result = this._kNearestNeighborsDomainService.GeneticOptimize(input, data);

            if (input.record)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>() { result });
        } 
        #endregion

        #region GetExamples
        public async Task<NbaAtsTreeExample[]> GetExamples()
        {
            NbaAtsTreeExample[] data;

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                data = this._exampleRepository.GetAll().Where(x => x.Date < Clock.Now.Date).ToArray();
                //data = this._exampleRepository.GetAll().Where(x => x.Date < Clock.Now.Date).OrderByDescending(x => x.StatLine.Sample.Date).Take(2500).ToArray();
                unitOfWork.Complete();
            }

            return data;
        }
        #endregion

        #region PredictToday
        public async Task PredictToday()
        {
            try
            {
                using (GuerillaTimer timer = new GuerillaTimer(this._consoleHubProxy))
                {
                    DecisionTree decisionTree = await this._decisionTreeRepository.FirstOrDefaultAsync(x => x.Target == DecisionTreeTargets.NbaAts);

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        DateTime now = Clock.Now;
                        List<NbaAtsTreeExample> todaysExamples = this._exampleRepository.GetAll().Where(x => x.Date.Year == now.Year && x.Date.Month == now.Month && x.Date.Day == now.Day).ToList();

                        foreach (NbaAtsTreeExample example in todaysExamples)
                        {
                            NbaStatLine statLine = await this._statLineRepository.GetAsync(example.StatLineId);
                            statLine.TreePoints = Double.Parse(this._decisionTreeDomainService.Classify(example, decisionTree.Root).OrderByDescending(x => x.count).First().value);
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
    }
}
