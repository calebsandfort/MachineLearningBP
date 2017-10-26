
using MachineLearningBP.CollectiveIntelligence.DomainServices.Examples;
using MachineLearningBP.Entities.iSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Repositories;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms;
using MachineLearningBP.Core.Services;
using MachineLearningBP.Shared.CommandRunner;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Shared.GuerillaTimer;

namespace MachineLearningBP.Services.iSupport
{
    public class IncidentExampleDomainService : MinimumExampleDomainService<Incident, IncidentStatLine, IncidentExample, Double, IncidentExampleGenerationInfo>, IIncidentExampleDomainService
    {
        #region Properties
        public readonly IKNearestNeighborsDomainService<IncidentExample, IncidentStatLine, Double> _kNearestNeighborsDomainService;
        private readonly ISheetUtilityDomainService _sheetUtilityDomainService;
        public readonly ICommandRunner _commandRunner;
        #endregion

        public IncidentExampleDomainService(IRepository<Incident> sampleRepository, IRepository<IncidentStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager,
            IRepository<IncidentExample> exampleRepository, IBackgroundJobManager backgroundJobManager,
            IKNearestNeighborsDomainService<IncidentExample, IncidentStatLine, Double> kNearestNeighborsDomainService,
            ICommandRunner commandRunner, ISheetUtilityDomainService sheetUtilityDomainService)
            : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, exampleRepository, backgroundJobManager)
        {
            this._kNearestNeighborsDomainService = kNearestNeighborsDomainService;
            this._sheetUtilityDomainService = sheetUtilityDomainService;
            this._commandRunner = commandRunner;
        }

        public async Task PopulateExamples()
        {
            await PopulateExamples(0, 0);
        }

        public async Task PopulateExamples(int rollingWindowPeriod, double scaleFactor)
        {
            try
            {
                using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
                {
                    this.DeleteExamples();

                    DateTime now = DateTime.Now.Date;
                    int count = 0;
                    List<Incident> incidents;

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        count = this._sampleRepository.Count();
                        await unitOfWork.CompleteAsync();
                    }

                    int pageSize = 250;
                    int pageCount = (int)Math.Ceiling((double)count / (double)pageSize);

                    for (int i = 0; i < pageCount; i++)
                    {
                        using (GuerillaTimer timer = new GuerillaTimer(this._consoleHubProxy, $"Incident Example Generation page {i + 1} of {pageCount}"))
                        {
                            using (var unitOfWork = this.UnitOfWorkManager.Begin())
                            {
                                incidents = this._sampleRepository.GetAllIncluding(x => x.StatLines).OrderBy(x => x.Id).Skip(i * pageSize).Take(pageSize).ToList();
                                await unitOfWork.CompleteAsync();
                            }

                            await Task.WhenAll(incidents.Select(x => PopulateExample(x)).ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }

        public async Task PopulateExample(Incident incident)
        {
            try
            {
                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    IncidentExampleGenerationInfo info = new IncidentExampleGenerationInfo(incident);
                    IncidentExample example = new IncidentExample();
                    example.SetFields(info.StatLine, info);
                    await this._exampleRepository.InsertAsync(example);

                    await unitOfWork.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }

        public async Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParametersPythonAndR(bool record)
        {
            IncidentExample[] data = await this.GetExamples();

            KNearestNeighborsOptimalParametersInput<IncidentExample, IncidentStatLine, Double> optimalParametersInput = new KNearestNeighborsOptimalParametersInput<IncidentExample, IncidentStatLine, Double>();
            optimalParametersInput.Data = data;
            optimalParametersInput.DistanceMethod = KNearestNeighborsDistanceMethods.Gower;
            optimalParametersInput.GuessMethods.Add(KNearestNeighborsGuessMethods.KnnEstimate);
            optimalParametersInput.GuessMethods.Add(KNearestNeighborsGuessMethods.WeightedKnn);

            optimalParametersInput.WeightMethods.Add(KNearestNeighborsWeightMethods.Gaussian);
            optimalParametersInput.WeightMethods.Add(KNearestNeighborsWeightMethods.InverseWeight);
            optimalParametersInput.WeightMethods.Add(KNearestNeighborsWeightMethods.SubtractWeight);

            optimalParametersInput.Ks = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 };
            optimalParametersInput.SubtractWeightConstant = 1;
            optimalParametersInput.Trials = 25;

            List<KNearestNeighborsCrossValidateResult> results = this._kNearestNeighborsDomainService.FindOptimalParametersPythonAndR(optimalParametersInput);

            if (record && results.Count > 0)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>(results));

            return results;
        }

        public async Task<IncidentExample[]> GetExamples()
        {
            IncidentExample[] data;

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                //data = this._exampleRepository.GetAll().Where(x => x.WideRelease < Clock.Now.Date && x.StatLine.TheaterCount > 1000).ToArray();
                data = this._exampleRepository.GetAll().OrderBy(x => x.Id).Take(1000).ToArray();
                unitOfWork.Complete();
            }

            return data;
        }

        public void DeleteExamples()
        {
            using (GuerillaTimer timer = new GuerillaTimer(this._consoleHubProxy))
            {
                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    this._sqlExecuter.Execute($"DELETE FROM [MovieOpeningWeekendExamples]");
                    unitOfWork.Complete();
                }
            }
        }
    }
}
