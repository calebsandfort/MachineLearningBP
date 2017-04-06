using Abp.Domain.Services;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices
{
    public interface IExampleDomainService : IDomainService
    {
        Task PopulateExamples(int rollingWindowPeriod, double scaleFactor);
        void DeleteExamples();
    }
}
