using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class OptimizeInput
    {
        public List<OptimizationRange> domain { get; set; }
        public Func<List<int>, double> costf { get; set; }
        public bool record { get; set; }
    }
}
