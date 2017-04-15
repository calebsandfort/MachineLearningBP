using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Entities.Sports.Nba;

namespace MachineLearningBP.Services.Sports.Nba
{
    public interface INbaPointsExampleDomainService : ISportExampleDomainService<NbaGame, NbaTeam, NbaPointsExample, NbaStatLine, double, GeneticOptimizeInput, AnnealingOptimizeInput>
    {
    }
}
