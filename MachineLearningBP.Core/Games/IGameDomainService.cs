using Abp.Domain.Services;
using MachineLearningBP.Seasons;
using System;
using System.Threading.Tasks;

namespace MachineLearningBP.Games
{
    public interface IGameDomainService : IDomainService
    {
        Task PopulateGames();
        void DeleteGames();
    }
}
