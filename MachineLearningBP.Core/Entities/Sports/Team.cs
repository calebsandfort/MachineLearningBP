using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.CollectiveIntelligence.Entities;

namespace MachineLearningBP.Entities.Sports
{
    public abstract class Team<TStatLine, TTimeGrouping> : ParticipantMinimum<SportStatLine<TStatLine, TTimeGrouping>>
        where TStatLine : StatLine
        where TTimeGrouping : TimeGrouping
    {
    }
}
