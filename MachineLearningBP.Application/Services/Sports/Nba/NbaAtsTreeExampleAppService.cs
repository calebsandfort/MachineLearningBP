using Abp.Configuration;
using Abp.Domain.Uow;
using MachineLearningBP.CollectiveIntelligence.ApplicationServices;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using System.Threading.Tasks;
using System;

namespace MachineLearningBP.Services.Sports.Nba
{
    public class NbaAtsTreeExampleAppService : BaseApplicationService, INbaAtsTreeExampleAppService
    {
        private readonly INbaAtsTreeExampleDomainService _nbaAtsTreeExampleDomainService;
        private readonly ISheetUtilityAppService _sheetUtilityAppService;

        public NbaAtsTreeExampleAppService(ISettingManager settingManager, INbaAtsTreeExampleDomainService nbaAtsTreeExampleDomainService, ISheetUtilityAppService sheetUtilityAppService) : base(settingManager)
        {
            _nbaAtsTreeExampleDomainService = nbaAtsTreeExampleDomainService;
            _sheetUtilityAppService = sheetUtilityAppService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PopulateExamples()
        {
            await this._nbaAtsTreeExampleDomainService.PopulateExamples(this._settingManager.GetSettingValue<int>("NbaRollingWindowPeriod"), this._settingManager.GetSettingValue<int>("NbaScaleFactor"));
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task KNearestNeighborsDoStuff()
        {
            await this._nbaAtsTreeExampleDomainService.KNearestNeighborsDoStuff();
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task FindOptimalParameters()
        {
            //List<KNearestNeighborsCrossValidateResult> results = await this._nbaAtsTreeExampleDomainService.FindOptimalParameters((r) => _sheetUtilityAppService.RecordKnnNbaAtsTreedOptimalParameters(r));
            //_sheetUtilityAppService.RecordKnnNbaAtsTreedOptimalParameters(results);

            await this._nbaAtsTreeExampleDomainService.FindOptimalParametersEnqueue(true);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task AnnealingOptimize(AnnealingOptimizeInput input)
        {
            await this._nbaAtsTreeExampleDomainService.AnnealingOptimize(input);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task GeneticOptimize(GeneticOptimizeInput input)
        {
            await this._nbaAtsTreeExampleDomainService.GeneticOptimizeEnqueue(input);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PredictToday()
        {
            await this._nbaAtsTreeExampleDomainService.PredictToday();
        }

        public Task FindOptimalParametersPython()
        {
            throw new NotImplementedException();
        }
    }
}
