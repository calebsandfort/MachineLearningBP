using Abp.Application.Services;
using System.Threading.Tasks;

namespace MachineLearningBP.Application.Services.Sports
{
    public interface ISeasonAppService : IApplicationService
    {
        Task SetSeasonsRollingWindowStart();
    }
}
