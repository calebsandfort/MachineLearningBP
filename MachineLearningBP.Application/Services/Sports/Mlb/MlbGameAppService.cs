using Abp.Domain.Uow;
using MachineLearningBP.Core.Services.Sports.Mlb;
using System.Threading.Tasks;

namespace MachineLearningBP.Application.Services.Sports.Mlb
{
    public class MlbGameAppService : IMlbGameAppService
    {
        private readonly IMlbGameDomainService _mlbGameDomainService;

        public MlbGameAppService(IMlbGameDomainService mlbGameDomainService)
        {
            _mlbGameDomainService = mlbGameDomainService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PopulateSamples()
        {
            await this._mlbGameDomainService.PopulateSamples();
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task FillSamples()
        {
            await this._mlbGameDomainService.FillSamples();
        }
    }
}
