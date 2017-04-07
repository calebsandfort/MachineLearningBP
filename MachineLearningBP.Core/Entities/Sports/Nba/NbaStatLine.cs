
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaStatLines")]
    public class NbaStatLine : StatLine, SportStatLine<NbaTeam, NbaGame>
    {
        [ForeignKey("SampleId")]
        public virtual NbaGame Sample { get; set; }
        public virtual int SampleId { get; set; }

        [ForeignKey("ParticipantId")]
        public virtual NbaTeam Participant { get; set; }
        public virtual int ParticipantId { get; set; }

        public double Points { get; set; }
        public bool Home { get; set; }

        public bool TwoInTwoDays { get; set; }
        public bool ThreeInFourDays { get; set; }
        public bool FourInFiveDays { get; set; }
        public bool FourInSixDays { get; set; }
        public bool FiveInSevenDays { get; set; }

        public Double FieldGoalsMade { get; set; }
        public Double FieldGoalsAttempted { get; set; }
        public Double ThreePointersMade { get; set; }
        public Double ThreePointersAttempted { get; set; }
        public Double FreeThrowsMade { get; set; }
        public Double FreeThrowsAttempted { get; set; }
        public Double Turnovers { get; set; }
        public Double OffensiveRebounds { get; set; }
        public Double DefensiveRebounds { get; set; }
        public Double TotalRebounds { get { return this.OffensiveRebounds + this.DefensiveRebounds; } }
    }
}
