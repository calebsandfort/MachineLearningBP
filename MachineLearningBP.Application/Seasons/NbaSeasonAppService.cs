using Abp.Domain.Uow;
using System.Threading.Tasks;

namespace MachineLearningBP.Seasons
{
    public class NbaSeasonAppService : INbaSeasonAppService
    {
        private readonly INbaSeasonDomainService _nbaSeasonDomainService;

        public NbaSeasonAppService(INbaSeasonDomainService nbaSeasonDomainService)
        {
            _nbaSeasonDomainService = nbaSeasonDomainService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task SetSeasonsRollingWindowStart()
        {
            await this._nbaSeasonDomainService.SetSeasonsRollingWindowStart();
        }
    }
}
