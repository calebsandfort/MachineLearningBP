using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Examples;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MachineLearningBP.Services.Sports
{
    public interface ISportExampleDomainService<TSample, TParticipant, TExample, TStatLine, TResult, TGeneticOptimizeInput, TAnnealingOptimizeInput> : IExampleDomainService
        where TSample : Sample
        where TParticipant : Participant
        where TExample : Example<TStatLine, TResult>
        where TStatLine : StatLine
        where TGeneticOptimizeInput: IGeneticOptimizeInput
        where TAnnealingOptimizeInput : IAnnealingOptimizeInput
    {
        Task PopulateExample(TSample game, List<TSample> games, List<TParticipant> teams, int rollingWindowPeriod, double scaleFactor);
        Task KNearestNeighborsDoStuff();
        Task FindOptimalParametersEnqueue(bool record);
        Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParameters(bool record);
        Task<DecisionNode> BuildDecisionTree();
        Task<TExample[]> GetExamples();

        Task AnnealingOptimizeEnqueue(TAnnealingOptimizeInput input);
        Task AnnealingOptimize(TAnnealingOptimizeInput input);

        Task GeneticOptimizeEnqueue(TGeneticOptimizeInput input);
        Task GeneticOptimize(TGeneticOptimizeInput input);

        Task PredictToday();
    }
}
