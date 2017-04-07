using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public interface SampleMedium<TTimeGrouping, TStatLine> : SampleMinimum<TStatLine>
        where TTimeGrouping : TimeGrouping
        where TStatLine : StatLine
    {
        [ForeignKey("TimeGroupingId")]
        TTimeGrouping TimeGrouping { get; set; }
        int TimeGroupingId { get; set; }
    }
}
