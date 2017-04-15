using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class OptimizationScore
    {
        public Double Cost { get; set; }
        public List<int> Vec { get; set; }
    }
}
