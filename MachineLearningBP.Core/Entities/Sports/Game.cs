using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.CollectiveIntelligence.Entities;

namespace MachineLearningBP.Entities.Sports
{
    public interface Game<TTimeGrouping, TStatLine> : SampleMedium<TTimeGrouping, TStatLine>
        where TTimeGrouping : TimeGrouping
        where TStatLine : StatLine
    {
        DateTime Date { get; set; }
        int EspnIdentifier { get; set; }
        bool Completed { get; set; }
        Double Total { get; set; }
    }
}
