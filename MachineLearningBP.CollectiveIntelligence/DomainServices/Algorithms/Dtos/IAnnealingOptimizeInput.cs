using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public interface IAnnealingOptimizeInput : IOptimizeInput
    {
        double T { get; set; }
        double cool { get; set; }
        int step { get; set; }
    }
}
