using Abp.Domain.Entities;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MachineLearningBP.Entities.Sports.Mlb
{
    [Table("MlbTeams")]
    public class MlbTeam : Participant, Team, ParticipantMinimum<MlbStatLine> 
    {
        [ForeignKey("ParticipantId")]
        public virtual ICollection<MlbStatLine> StatLines { get; set; }
    }
}
