using Abp.Configuration;
using Abp.Domain.Uow;
using MachineLearningBP.CollectiveIntelligence.ApplicationServices;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using System.Threading.Tasks;
using System;

namespace MachineLearningBP.Services.Sports.Nba
{
    public class NbaPointsExampleAppService : BaseApplicationService, INbaPointsExampleAppService
    {
        private readonly INbaPointsExampleDomainService _nbaPointsExampleDomainService;
        private readonly ISheetUtilityAppService _sheetUtilityAppService;

        public NbaPointsExampleAppService(ISettingManager settingManager, INbaPointsExampleDomainService nbaPointsExampleDomainService, ISheetUtilityAppService sheetUtilityAppService) : base(settingManager)
        {
            _nbaPointsExampleDomainService = nbaPointsExampleDomainService;
            _sheetUtilityAppService = sheetUtilityAppService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PopulateExamples()
        {
            await this._nbaPointsExampleDomainService.PopulateExamples(this._settingManager.GetSettingValue<int>("NbaRollingWindowPeriod"), this._settingManager.GetSettingValue<int>("NbaScaleFactor"));
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task KNearestNeighborsDoStuff()
        {
            await this._nbaPointsExampleDomainService.KNearestNeighborsDoStuff();
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task FindOptimalParameters()
        {
            //List<KNearestNeighborsCrossValidateResult> results = await this._nbaPointsExampleDomainService.FindOptimalParameters((r) => _sheetUtilityAppService.RecordKnnNbaPointsdOptimalParameters(r));
            //_sheetUtilityAppService.RecordKnnNbaPointsdOptimalParameters(results);

            await this._nbaPointsExampleDomainService.FindOptimalParametersEnqueue(true);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task AnnealingOptimize(AnnealingOptimizeInput input)
        {
            await this._nbaPointsExampleDomainService.AnnealingOptimize(input);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task GeneticOptimize(GeneticOptimizeInput input)
        {
            await this._nbaPointsExampleDomainService.GeneticOptimizeEnqueue(input);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PredictToday()
        {
            await this._nbaPointsExampleDomainService.PredictToday();
        }
    }
}
