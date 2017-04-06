using System;
using MachineLearningBP.CollectiveIntelligence.Entities;

namespace MachineLearningBP.Entities.Sports
{
    public abstract class Season<TStatLine, TTimeGrouping> : TimeGroupingMinimum<Game<TStatLine, TTimeGrouping>>
        where TStatLine : StatLine
        where TTimeGrouping : TimeGrouping
    {
        public DateTime? RollingWindowStart { get; set; }
    }
}
