using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Domain.Entities;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaTeams")]
    public class NbaTeam : Participant, Team, ParticipantMinimum<NbaStatLine>
    {
        [ForeignKey("ParticipantId")]
        public virtual ICollection<NbaStatLine> StatLines { get; set; }
    }
}
