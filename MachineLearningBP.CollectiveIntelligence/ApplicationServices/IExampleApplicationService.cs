using Abp.Application.Services;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.ApplicationServices
{
    public interface IExampleAppService : IApplicationService
    {
        Task PopulateExamples();
    }
}
