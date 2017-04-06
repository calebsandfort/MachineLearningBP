using Abp.Domain.Uow;
using MachineLearningBP.Core.Services.Sports.Mlb;
using System.Threading.Tasks;

namespace MachineLearningBP.Application.Services.Sports.Mlb
{
    public class MlbSeasonAppService : IMlbSeasonAppService
    {
        private readonly IMlbSeasonDomainService _mlbSeasonDomainService;

        public MlbSeasonAppService(IMlbSeasonDomainService mlbSeasonDomainService)
        {
            _mlbSeasonDomainService = mlbSeasonDomainService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task SetSeasonsRollingWindowStart()
        {
            await this._mlbSeasonDomainService.SetSeasonsRollingWindowStart();
        }
    }
}
