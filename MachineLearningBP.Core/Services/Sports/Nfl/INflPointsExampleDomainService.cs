using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Entities.Sports.Nfl;

namespace MachineLearningBP.Services.Sports.Nfl
{
    public interface INflPointsExampleDomainService : ISportExampleDomainService<NflGame, NflTeam, NflPointsExample, NflStatLine, double, GeneticOptimizeInput, AnnealingOptimizeInput>
    {
    }
}
