using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public class ValueIndexPair<TValue>
    {
        public TValue Value { get; set; }
        public int Index { get; set; }

        public ValueIndexPair(TValue value, int index)
        {
            this.Value = value;
            this.Index = index;
        }

        public override string ToString()
        {
            return $"{this.Index}, {this.Value:N4}";
        }
    }
}
