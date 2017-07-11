using Abp.Domain.Uow;
using MachineLearningBP.Core.Services.Sports.Nfl;
using System.Threading.Tasks;

namespace MachineLearningBP.Application.Services.Sports.Nfl
{
    public class NflGameAppService : INflGameAppService
    {
        private readonly INflGameDomainService _nflGameDomainService;

        public NflGameAppService(INflGameDomainService nflGameDomainService)
        {
            _nflGameDomainService = nflGameDomainService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PopulateSamples()
        {
            await this._nflGameDomainService.PopulateSamples();
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task FillSamples()
        {
            await this._nflGameDomainService.FillSamples();
        }
    }
}
