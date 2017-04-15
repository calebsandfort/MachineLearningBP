using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public interface IRecordContainer
    {
        List<String> ColumnHeaders { get; }
        List<String> ColumnValues { get; }
    }
}
