using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Examples;
using MachineLearningBP.Entities.iSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Services.iSupport
{
    public interface IIncidentExampleDomainService : IExampleDomainService
    {
        Task PopulateExamples();
        Task PopulateExample(Incident incident);
        Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParametersPythonAndR(bool record);
        Task<IncidentExample[]> GetExamples();
    }
}
