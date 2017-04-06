using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.CollectiveIntelligence.Entities;

namespace MachineLearningBP.Entities.Sports
{
    public abstract class Game<TStatLine, TTimeGrouping> : SampleMedium<TStatLine, TTimeGrouping>
        where TStatLine : StatLine
        where TTimeGrouping: TimeGrouping
    {
        public DateTime Date { get; set; }
        public int EspnIdentifier { get; set; }
        public bool Completed { get; set; }
        public Double Total { get; set; }
    }
}
