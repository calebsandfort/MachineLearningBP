
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Mlb
{
    [Table("MlbStatLines")]
    public class MlbStatLine : StatLine, SportStatLine<MlbTeam, MlbGame>
    {
        [ForeignKey("SampleId")]
        public virtual MlbGame Sample { get; set; }
        public virtual int SampleId { get; set; }

        [ForeignKey("ParticipantId")]
        public virtual MlbTeam Participant { get; set; }
        public virtual int ParticipantId { get; set; }

        public double Points { get; set; }
        public bool Home { get; set; }

        public Double Moneyline { get; set; }
        public Double InningsPitched { get; set; }
        public Double AtBats { get; set; }
        public Double Walks { get; set; }
        public Double Hits { get; set; }
        public Double HitByPitch { get; set; }
        public Double SacrificeFlies { get; set; }
    }
}
