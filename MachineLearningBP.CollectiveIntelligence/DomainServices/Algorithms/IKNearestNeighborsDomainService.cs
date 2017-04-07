using Abp.Domain.Services;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms
{
    public interface IKNearestNeighborsDomainService<TResult> : IDomainService
    {
        void DivideData(List<Example<TResult>> data, out List<Example<TResult>> trainSet, out List<Example<TResult>> testSet, Double test = .05);
    }
}
