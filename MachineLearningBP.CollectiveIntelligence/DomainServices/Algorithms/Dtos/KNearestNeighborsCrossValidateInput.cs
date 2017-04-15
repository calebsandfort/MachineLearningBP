using MachineLearningBP.CollectiveIntelligence.Entities;
using System.Collections.Generic;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>
        where TExample : Example<TStatLine, TResult>
        where TStatLine : StatLine
    {
        public TExample[] Data { get; set; }
        public KNearestNeighborsGuessMethods GuessMethod { get; set; }
        public KNearestNeighborsWeightMethods WeightMethod { get; set; }
        public int Trials { get; set; }
        public int[] Ks { get; set; }

        //InverseWeight
        public double InverseWeightNum { get; set; }
        public double InverseWeightConstant { get; set; }

        //SubtractWeight
        public double SubtractWeightConstant { get; set; }

        //Gaussian
        public double GaussianSigma { get; set; }

        public KNearestNeighborsCrossValidateInput()
        {
            this.Trials = 100;
            this.Ks = new int[] { 5 };

            this.WeightMethod = KNearestNeighborsWeightMethods.None;
            this.InverseWeightNum = 1.0;
            this.InverseWeightConstant = .1;
            this.SubtractWeightConstant = 1.0;
            this.GaussianSigma = 5.0;
        }

        public override string ToString()
        {
            return ($"{this.GuessMethod}{(this.GuessMethod == KNearestNeighborsGuessMethods.WeightedKnn ? ", " + this.WeightMethod : string.Empty)}");
        }
    }

    public enum KNearestNeighborsGuessMethods
    {
        KnnEstimate,
        WeightedKnn
    }

    public enum KNearestNeighborsWeightMethods
    {
        None,
        InverseWeight,
        SubtractWeight,
        Gaussian
    }
}
