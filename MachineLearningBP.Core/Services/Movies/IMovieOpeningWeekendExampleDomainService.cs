using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Examples;
using MachineLearningBP.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Services.Movies
{
    public interface IMovieOpeningWeekendExampleDomainService : IExampleDomainService
    {
        Task PopulateExamples();
        Task PopulateExample(Movie movie);
        //Task KNearestNeighborsDoStuff();
        //Task FindOptimalParametersEnqueue(bool record);
        //Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParameters(bool record);
        //Task FindOptimalParametersPythonEnqueue(bool record);
        Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParametersPythonAndR(bool record);
        //Task<DecisionNode> BuildDecisionTree();
        Task<MovieOpeningWeekendExample[]> GetExamples();

        //Task AnnealingOptimizeEnqueue(TAnnealingOptimizeInput input);
        //Task AnnealingOptimize(TAnnealingOptimizeInput input);

        //Task GeneticOptimizeEnqueue(TGeneticOptimizeInput input);
        //Task GeneticOptimize(TGeneticOptimizeInput input);

        //Task PredictToday();
    }
}
