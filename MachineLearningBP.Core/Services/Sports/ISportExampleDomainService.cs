using MachineLearningBP.CollectiveIntelligence.DomainServices;
using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Entities.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Services.Sports
{
    public interface ISportExampleDomainService<TSample, TParticipant> : IExampleDomainService
        where TSample : Sample
        where TParticipant : Participant
    {
        Task PopulateExample(TSample game, List<TSample> games, List<TParticipant> teams, int rollingWindowPeriod, double scaleFactor);
    }
}
