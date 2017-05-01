using Abp.Domain.Services;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms
{
    public interface IKNearestNeighborsDomainService<TExample, TStatLine, TResult> : IDomainService
        where TExample : Example<TStatLine, TResult>
        where TStatLine : StatLine
    {
        void DivideData(TExample[] data, out TExample[] trainSet, out TExample[] testSet, Double test = .05);
        ValueIndexPair<double> Euclidean(TExample v1, TExample v2, int index);
        ValueIndexPair<Double>[][] GetDistances(TExample[] data, TExample[] v1);
        double[][] KnnEstimate(TExample[] data, TExample[] v1, int[] ks);
        double[][] WeightedKnn(TExample[] data, TExample[] v1, Func<Double, Double> weightf, int[] ks);
        Double InverseWeight(Double dist, Double num = 1.0, Double constant = .1);
        Double SubtractWeight(Double dist, Double constant = 1.0);
        Double Gaussian(Double dist, Double sigma = 5.0);
        Double[] TestAlgorithm(Func<TExample[], TExample[], int[], Double[][]> algf, TExample[] trainSet, TExample[] testSet, int[] ks);
        List<KNearestNeighborsCrossValidateResult> CrossValidate(KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> input, bool doTimer = true);
        Double[] CrossValidate(Func<TExample[], TExample[], int[], Double[][]> algf, TExample[] data, int[] ks, int trials = 100, Double test = .05);
        List<KNearestNeighborsCrossValidateResult> FindOptimalParameters(TExample[] data);
        OptimizeResult AnnealingOptimize(AnnealingOptimizeInput input, TExample[] data);
        OptimizeResult GeneticOptimize(GeneticOptimizeInput input, TExample[] data);
        TExample[] Rescale(TExample[] data, List<int> scale);
        Func<List<int>, int, double> CreateCostFunction(KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> input);


        List<KNearestNeighborsCrossValidateResult> FindOptimalParametersPython(TExample[] data);
        List<KNearestNeighborsCrossValidateResult> FindOptimalParametersPythonAndR(TExample[] data);

        void WritePythonDataFile(TExample[] data, Double[] gowerDistances = null, Double resultScale = 1);
        void WriteRDataFile(TExample[] data);
        Double[] GetGowerDistances(TExample[] data);
    }
}
