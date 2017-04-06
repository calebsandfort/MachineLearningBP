using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public abstract class TimeGrouping : Entity<int>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool Within(DateTime date)
        {
            return (date >= this.Start && date <= this.End);
        }
    }
}
