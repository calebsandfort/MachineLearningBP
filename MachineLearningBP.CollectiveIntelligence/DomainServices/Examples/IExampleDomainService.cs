using Abp.Domain.Services;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Examples
{
    public interface IExampleDomainService : IDomainService
    {
        Task PopulateExamples(int rollingWindowPeriod, double scaleFactor);
        void DeleteExamples();
    }
}
