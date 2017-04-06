using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.CollectiveIntelligence.Entities;

namespace MachineLearningBP.Entities.Sports
{
    public abstract class SportStatLine<TStatLine, TTimeGrouping> : StatLineMedium<Game<TStatLine, TTimeGrouping>, Team<TStatLine, TTimeGrouping>>
        where TStatLine : StatLine
        where TTimeGrouping : TimeGrouping
    {
        public Double Points { get; set; }
        public bool Home { get; set; }
    }
}
