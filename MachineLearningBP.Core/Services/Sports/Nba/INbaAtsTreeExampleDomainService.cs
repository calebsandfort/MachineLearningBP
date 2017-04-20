using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Entities.Sports.Nba;
using System;

namespace MachineLearningBP.Services.Sports.Nba
{
    public interface INbaAtsTreeExampleDomainService : ISportExampleDomainService<NbaGame, NbaTeam, NbaAtsTreeExample, NbaStatLine, Double, GeneticOptimizeInput, AnnealingOptimizeInput>
    {
    }
}
