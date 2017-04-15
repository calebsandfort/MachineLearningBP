using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class OptimizeResult : IRecordContainer
    {
        public List<int> Vec { get; set; }

        public List<string> ColumnHeaders
        {
            get
            {
                List<String> columnHeaders = new List<string>();

                columnHeaders.Add("Vec");

                return columnHeaders;
            }
        }

        public List<string> ColumnValues
        {
            get
            {
                List<String> columnValues = new List<string>();
                columnValues.Add(String.Join(",", this.Vec));
                return columnValues;
            }
        }
    }
}
