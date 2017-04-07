using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.CollectiveIntelligence.Entities;

namespace MachineLearningBP.Entities.Sports
{
    public interface SportStatLine<TParticipant, TSample> : StatLineMedium<TParticipant, TSample>
        where TParticipant : Participant
        where TSample : Sample
    {
        Double Points { get; set; }
        bool Home { get; set; }
    }
}
