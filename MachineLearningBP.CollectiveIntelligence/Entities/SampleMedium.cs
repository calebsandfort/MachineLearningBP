using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public abstract class SampleMedium<TStatLine, TTimeGrouping> : SampleMinimum<TStatLine>
        where TStatLine : StatLine
        where TTimeGrouping : TimeGrouping
    {
        [ForeignKey("TimeGroupingId")]
        public virtual TTimeGrouping TimeGrouping { get; set; }
        public virtual int TimeGroupingId { get; set; }
    }
}
