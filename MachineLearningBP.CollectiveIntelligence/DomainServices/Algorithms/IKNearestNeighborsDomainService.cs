using Abp.Domain.Services;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms
{
    public interface IKNearestNeighborsDomainService<TExample, TStatLine, TResult> : IDomainService
        where TExample : Example<TStatLine, TResult>
        where TStatLine : StatLine
    {
        void DivideData(List<TExample> data, out List<TExample> trainSet, out List<TExample> testSet, Double test = .05);
    }
}
