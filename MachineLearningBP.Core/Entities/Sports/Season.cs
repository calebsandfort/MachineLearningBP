using System;
using MachineLearningBP.CollectiveIntelligence.Entities;

namespace MachineLearningBP.Entities.Sports
{
    public interface Season<TSample> : TimeGroupingMinimum<TSample>
        where TSample : Sample
    {
        DateTime? RollingWindowStart { get; set; }
    }
}
