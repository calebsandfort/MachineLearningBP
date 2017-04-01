using Abp.Application.Services;
using System.Threading.Tasks;

namespace MachineLearningBP.Seasons
{
    public interface ISeasonAppService : IApplicationService
    {
        Task SetSeasonsRollingWindowStart();
    }
}
