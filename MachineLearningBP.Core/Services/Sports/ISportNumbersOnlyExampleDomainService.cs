using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Entities.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Services.Sports
{
    public interface ISportNumbersOnlyExampleDomainService<TSample, TStatLine, TTimeGrouping, TParticipant>
        where TSample : Game<TStatLine, TTimeGrouping>
        where TStatLine : SportStatLine<TStatLine, TTimeGrouping>
        where TTimeGrouping : Season<TStatLine, TTimeGrouping>
        where TParticipant : Participant
    {
        Task PopulateExample(TSample game, List<TSample> games, List<TParticipant> teams, int rollingWindowPeriod, double scaleFactor);
    }
}
