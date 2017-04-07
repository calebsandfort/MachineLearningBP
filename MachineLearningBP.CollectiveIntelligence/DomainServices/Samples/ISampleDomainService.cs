using Abp.Domain.Services;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Samples
{
    public interface ISampleDomainService : IDomainService
    {
        Task PopulateSamples();
        void DeleteSamples();
    }
}
