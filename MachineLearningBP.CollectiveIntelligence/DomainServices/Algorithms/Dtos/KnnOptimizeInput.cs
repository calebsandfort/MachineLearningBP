using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class KnnOptimizeInput : OptimizeInput
    {
        public KNearestNeighborsGuessMethods GuessMethod { get; set; }
        public KNearestNeighborsWeightMethods WeightMethod { get; set; }
        public int Trials { get; set; }
        public int K { get; set; }
    }
}
