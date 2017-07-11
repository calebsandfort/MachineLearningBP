using Abp.Domain.Uow;
using MachineLearningBP.Core.Services.Sports.Nfl;
using System.Threading.Tasks;

namespace MachineLearningBP.Application.Services.Sports.Nfl
{
    public class NflSeasonAppService : INflSeasonAppService
    {
        private readonly INflSeasonDomainService _nflSeasonDomainService;

        public NflSeasonAppService(INflSeasonDomainService nflSeasonDomainService)
        {
            _nflSeasonDomainService = nflSeasonDomainService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task SetSeasonsRollingWindowStart()
        {
            await this._nflSeasonDomainService.SetSeasonsRollingWindowStart();
        }
    }
}
