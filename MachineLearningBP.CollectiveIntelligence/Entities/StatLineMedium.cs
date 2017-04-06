using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public abstract class StatLineMedium<TSample, TParticipant> : StatLineMinimum<TSample>
        where TSample : Sample
        where TParticipant : Participant
    {
        [ForeignKey("ParticipantId")]
        public virtual TParticipant Participant { get; set; }
        public virtual int ParticipantId { get; set; }
    }
}
