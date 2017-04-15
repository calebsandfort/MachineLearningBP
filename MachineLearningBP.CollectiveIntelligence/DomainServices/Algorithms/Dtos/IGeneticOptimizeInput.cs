using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public interface IGeneticOptimizeInput : IOptimizeInput
    {
        int popsize { get; set; }
        int step { get; set; }
        double mutprob { get; set; }
        double elite { get; set; }
        int maxiter { get; set; }
    }
}
