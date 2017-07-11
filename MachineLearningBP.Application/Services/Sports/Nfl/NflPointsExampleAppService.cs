using Abp.Configuration;
using Abp.Domain.Uow;
using MachineLearningBP.CollectiveIntelligence.ApplicationServices;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using System.Threading.Tasks;
using System;

namespace MachineLearningBP.Services.Sports.Nfl
{
    public class NflPointsExampleAppService : BaseApplicationService, INflPointsExampleAppService
    {
        private readonly INflPointsExampleDomainService _nflPointsExampleDomainService;
        private readonly ISheetUtilityAppService _sheetUtilityAppService;

        public NflPointsExampleAppService(ISettingManager settingManager, INflPointsExampleDomainService nflPointsExampleDomainService, ISheetUtilityAppService sheetUtilityAppService) : base(settingManager)
        {
            _nflPointsExampleDomainService = nflPointsExampleDomainService;
            _sheetUtilityAppService = sheetUtilityAppService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PopulateExamples()
        {
            await this._nflPointsExampleDomainService.PopulateExamples(this._settingManager.GetSettingValue<int>("NflRollingWindowPeriod"), 1);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task KNearestNeighborsDoStuff()
        {
            await this._nflPointsExampleDomainService.KNearestNeighborsDoStuff();
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task FindOptimalParameters()
        {
            //List<KNearestNeighborsCrossValidateResult> results = await this._nflPointsExampleDomainService.FindOptimalParameters((r) => _sheetUtilityAppService.RecordKnnNflPointsdOptimalParameters(r));
            //_sheetUtilityAppService.RecordKnnNflPointsdOptimalParameters(results);

            await this._nflPointsExampleDomainService.FindOptimalParametersEnqueue(true);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task FindOptimalParametersPython()
        {
            await this._nflPointsExampleDomainService.FindOptimalParametersPythonEnqueue(true);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task AnnealingOptimize(AnnealingOptimizeInput input)
        {
            await this._nflPointsExampleDomainService.AnnealingOptimize(input);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task GeneticOptimize(GeneticOptimizeInput input)
        {
            await this._nflPointsExampleDomainService.GeneticOptimizeEnqueue(input);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PredictToday()
        {
            await this._nflPointsExampleDomainService.PredictToday();
        }
    }
}
