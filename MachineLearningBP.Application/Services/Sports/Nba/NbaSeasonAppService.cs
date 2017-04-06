using Abp.Domain.Uow;
using MachineLearningBP.Core.Services.Sports.Nba;
using System.Threading.Tasks;

namespace MachineLearningBP.Application.Services.Sports.Nba
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
