using MachineLearningBP.Entities.Sports.Nba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.DataPointMethods
{
    public static class NbaMethods
    {
        public static Double CalculateShooting(List<NbaStatLine> statLines)
        {
            return (statLines.Sum(x => x.FieldGoalsMade) + (.5 * statLines.Sum(x => x.ThreePointersMade))) / statLines.Sum(x => x.FieldGoalsAttempted);
        }

        public static Double CalculateTurnovers(List<NbaStatLine> statLines)
        {
            return statLines.Sum(x => x.Turnovers) / (statLines.Sum(x => x.FieldGoalsAttempted) + (.44 * statLines.Sum(x => x.FreeThrowsAttempted)) + statLines.Sum(x => x.Turnovers));
        }

        public static Double CalculateRebounding(List<NbaStatLine> statLines, List<NbaStatLine> opponentStatLines)
        {
            return statLines.Sum(x => x.OffensiveRebounds) / (statLines.Sum(x => x.OffensiveRebounds) + opponentStatLines.Sum(x => x.DefensiveRebounds));
        }

        public static Double CalculateFreeThrows(List<NbaStatLine> statLines)
        {
            return statLines.Sum(x => x.FreeThrowsMade) / statLines.Sum(x => x.FieldGoalsAttempted);
        }
    }
}
