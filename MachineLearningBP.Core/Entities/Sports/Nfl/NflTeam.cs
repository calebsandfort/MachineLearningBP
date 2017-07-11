using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Domain.Entities;

namespace MachineLearningBP.Entities.Sports.Nfl
{
    [Table("NflTeams")]
    public class NflTeam : Participant, Team, ParticipantMinimum<NflStatLine>
    {
        [ForeignKey("ParticipantId")]
        public virtual ICollection<NflStatLine> StatLines { get; set; }

        public String PfrId { get; set; }
        public String SavId { get; set; }
    }
}
