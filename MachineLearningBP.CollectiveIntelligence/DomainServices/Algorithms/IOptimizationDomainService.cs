using Abp.Domain.Services;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms
{
    public interface IOptimizationDomainService : IDomainService
    {
        OptimizeResult AnnealingOptimize(IAnnealingOptimizeInput input);
        OptimizeResult AnnealingOptimize(List<OptimizationRange> domain, Func<List<int>, Double> costf, Double T = 10000.0, Double cool = .95, int step = 1);

        OptimizeResult GeneticOptimize(IGeneticOptimizeInput input);
        OptimizeResult GeneticOptimize(List<OptimizationRange> domain, Func<List<int>, Double> costf, int popsize= 50, int step = 1, Double mutprob = 0.2, Double elite = 0.2, int maxiter = 100);
    }
}
