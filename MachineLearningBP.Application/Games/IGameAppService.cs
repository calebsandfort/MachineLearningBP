using Abp.Application.Services;
using System.Threading.Tasks;

namespace MachineLearningBP.Games
{
    public interface IGameAppService : IApplicationService
    {
        Task PopulateGames();
    }
}
