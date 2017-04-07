using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public interface StatLineMedium<TParticipant, TSample> : StatLineMinimum<TSample>
        where TParticipant : Participant
        where TSample : Sample
    {
        //[ForeignKey("ParticipantId")]
        TParticipant Participant { get; set; }
        int ParticipantId { get; set; }
    }
}
