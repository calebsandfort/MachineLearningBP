using Abp.Application.Services;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.ApplicationServices
{
    public interface ISampleAppService : IApplicationService
    {
        Task PopulateSamples();
    }
}
