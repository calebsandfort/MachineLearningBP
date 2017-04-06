using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public abstract class TimeGroupingMinimum<Sample> : TimeGrouping
    {
        [ForeignKey("TimeGroupingId")]
        public virtual ICollection<Sample> Samples { get; set; }
    }
}
