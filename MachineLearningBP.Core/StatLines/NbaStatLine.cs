using MachineLearningBP.Games;
using MachineLearningBP.Teams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.StatLines
{
    [Table("NbaStatLines")]
    public class NbaStatLine : StatLine
    {
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

        [ForeignKey("TeamId")]
        public virtual NbaTeam Team { get; set; }
        public virtual int TeamId { get; set; }

        [ForeignKey("GameId")]
        public virtual NbaGame Game { get; set; }
        public virtual int GameId { get; set; }
    }
}
