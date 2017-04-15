using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class AnnealingOptimizeInput : KnnOptimizeInput, IAnnealingOptimizeInput
    {
        #region Properties
        public double T { get; set; }
        public double cool { get; set; }
        public int step { get; set; }
        #endregion

        #region Constructor
        public AnnealingOptimizeInput()
        {
            T = 10000.0;
            cool = .95;
            step = 1;
        }
        #endregion
    }
}
