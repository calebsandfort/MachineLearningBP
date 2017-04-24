using MachineLearningBP.CollectiveIntelligence.ApplicationServices;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using System.Threading.Tasks;

namespace MachineLearningBP.Application.Services.Sports
{
    public interface ISportExampleAppService : IExampleAppService
    {
        Task KNearestNeighborsDoStuff();
        Task FindOptimalParameters();
        Task FindOptimalParametersPython();
        Task AnnealingOptimize(AnnealingOptimizeInput input);
        Task GeneticOptimize(GeneticOptimizeInput input);
        Task PredictToday();
    }
}
