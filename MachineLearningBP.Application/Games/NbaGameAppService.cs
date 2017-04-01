using Abp.Domain.Uow;
using System.Threading.Tasks;

namespace MachineLearningBP.Games
{
    public class NbaGameAppService : INbaGameAppService
    {
        private readonly INbaGameDomainService _nbaGameDomainService;

        public NbaGameAppService(INbaGameDomainService nbaGameDomainService)
        {
            _nbaGameDomainService = nbaGameDomainService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PopulateGames()
        {
            await this._nbaGameDomainService.PopulateGames();
        }
    }
}
