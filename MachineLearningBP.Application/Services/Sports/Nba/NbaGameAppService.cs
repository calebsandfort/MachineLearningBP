using Abp.Domain.Uow;
using MachineLearningBP.Core.Services.Sports.Nba;
using System.Threading.Tasks;

namespace MachineLearningBP.Application.Services.Sports.Nba
{
    public class NbaGameAppService : INbaGameAppService
    {
        private readonly INbaGameDomainService _nbaGameDomainService;

        public NbaGameAppService(INbaGameDomainService nbaGameDomainService)
        {
            _nbaGameDomainService = nbaGameDomainService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PopulateSamples()
        {
            await this._nbaGameDomainService.PopulateSamples();
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task FillSamples()
        {
            await this._nbaGameDomainService.FillSamples();
        }
    }
}
