using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class KNearestNeighborsOptimalParametersInput<TExample, TStatLine, TResult>
        where TExample : Example<TStatLine, TResult>
        where TStatLine : StatLine
    {
        public TExample[] Data { get; set; }
        public List<KNearestNeighborsGuessMethods> GuessMethods { get; set; }
        public List<KNearestNeighborsWeightMethods> WeightMethods { get; set; }
        public KNearestNeighborsDistanceMethods DistanceMethod { get; set; }
        public int Trials { get; set; }
        public int[] Ks { get; set; }

        //InverseWeight
        public double InverseWeightNum { get; set; }
        public double InverseWeightConstant { get; set; }

        //SubtractWeight
        public double SubtractWeightConstant { get; set; }

        //Gaussian
        public double GaussianSigma { get; set; }

        public double ResultScale { get; set; }

        public KNearestNeighborsOptimalParametersInput()
        {
            this.Ks = new int[] { 5 };
            this.GuessMethods = new List<KNearestNeighborsGuessMethods>();
            this.WeightMethods = new List<KNearestNeighborsWeightMethods>();
            this.ResultScale = 1;
        }
    }
}
