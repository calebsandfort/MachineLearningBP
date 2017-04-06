using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports
{
    public abstract class SportNumbersOnlyExample<TExampleGenerationInfo, TSample, TStatLine, TTimeGrouping, TParticipant> : NumbersOnlyExample<TExampleGenerationInfo>
        where TExampleGenerationInfo : SportExampleGenerationInfo<TSample, TStatLine, TTimeGrouping, TParticipant>
        where TSample : Game<TStatLine, TTimeGrouping>
        where TStatLine : SportStatLine<TStatLine, TTimeGrouping>
        where TTimeGrouping : Season<TStatLine, TTimeGrouping>
        where TParticipant : Participant
    {

    }
}
