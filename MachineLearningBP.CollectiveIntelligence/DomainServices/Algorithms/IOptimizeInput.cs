using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using System;
using System.Collections.Generic;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms
{
    public interface IOptimizeInput
    {
        List<OptimizationRange> domain { get; set; }
        Func<List<int>, double> costf { get; set; }
    }
}
