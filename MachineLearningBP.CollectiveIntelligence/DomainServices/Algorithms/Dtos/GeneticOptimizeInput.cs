using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class GeneticOptimizeInput : KnnOptimizeInput,  IGeneticOptimizeInput
    {
        #region Properties
        public int popsize { get; set; }
        public int step { get; set; }
        public double mutprob { get; set; }
        public double elite { get; set; }
        public int maxiter { get; set; }
        #endregion

        #region Constructor
        public GeneticOptimizeInput()
        {
            popsize = 50;
            step = 1;
            mutprob = 0.2;
            elite = 0.2;
            maxiter = 100;
            record = true;
        }
        #endregion
    }
}
