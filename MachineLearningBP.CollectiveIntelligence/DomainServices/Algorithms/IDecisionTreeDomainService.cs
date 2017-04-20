using Abp.Domain.Services;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms
{
    public interface IDecisionTreeDomainService<TExample, TStatLine, TResult> : IDomainService
        where TExample : Example<TStatLine, TResult>
        where TStatLine : StatLine
    {
        DecisionNode BuildTree(List<TExample> rows, Func<List<TExample>, Double> scoref, int depth = 0);
        void Prune(DecisionNode tree, Func<List<TExample>, Double> scoref, Double mingain = .1);
        List<TreeResult> Classify(TExample observation, DecisionNode tree);
        void DivideSet(List<TExample> rows, out List<TExample> set1, out List<TExample> set2, int column, String value);
        List<TreeResult> UniqueCounts(List<TExample> rows);
        Double Variance(List<TExample> rows);
        Double GiniImpurity(List<TExample> rows);
        Double Entropy(List<TExample> rows);
    }
}
