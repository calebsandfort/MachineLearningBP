using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.DataPointMethods;
using MachineLearningBP.CollectiveIntelligence.Entities;

namespace MachineLearningBP.Entities.Sports.Nba
{
    public abstract class NbaFourFactorsNumbersOnlyExample : ExampleMinimum<Double, NbaExampleGenerationInfo>
    {
        public override void SetData(NbaExampleGenerationInfo info)
        {
            List<Double> dataPoints = new List<double>();

            #region Shooting
            //Offense
            dataPoints.Add(NbaMethods.CalculateShooting(info.Team1LastNStatLines));

            //Defense
            dataPoints.Add(NbaMethods.CalculateShooting(info.Team1LastNOpponentStatLines));

            //Opponent Offense
            dataPoints.Add(NbaMethods.CalculateShooting(info.Team2LastNStatLines));

            //Opponent Defense
            dataPoints.Add(NbaMethods.CalculateShooting(info.Team2LastNOpponentStatLines));
            #endregion

            #region Turnovers
            //Offense
            dataPoints.Add(NbaMethods.CalculateTurnovers(info.Team1LastNStatLines));

            //Defense
            dataPoints.Add(NbaMethods.CalculateTurnovers(info.Team1LastNOpponentStatLines));

            //Opponent Offense
            dataPoints.Add(NbaMethods.CalculateTurnovers(info.Team2LastNStatLines));

            //Opponent Defense
            dataPoints.Add(NbaMethods.CalculateTurnovers(info.Team2LastNOpponentStatLines));
            #endregion

            #region Rebounding
            //Offense
            dataPoints.Add(NbaMethods.CalculateRebounding(info.Team1LastNStatLines, info.Team1LastNOpponentStatLines));

            //Defense
            dataPoints.Add(NbaMethods.CalculateRebounding(info.Team1LastNStatLines, info.Team1LastNOpponentStatLines));

            //Opponent Offense
            dataPoints.Add(NbaMethods.CalculateRebounding(info.Team2LastNStatLines, info.Team2LastNOpponentStatLines));

            //Opponent Defense
            dataPoints.Add(NbaMethods.CalculateRebounding(info.Team2LastNStatLines, info.Team2LastNOpponentStatLines));
            #endregion

            #region Free Throws
            //Offense
            dataPoints.Add(NbaMethods.CalculateFreeThrows(info.Team1LastNStatLines));

            //Defense
            dataPoints.Add(NbaMethods.CalculateFreeThrows(info.Team1LastNOpponentStatLines));

            //Opponent Offense
            dataPoints.Add(NbaMethods.CalculateFreeThrows(info.Team2LastNStatLines));

            //Opponent Defense
            dataPoints.Add(NbaMethods.CalculateFreeThrows(info.Team2LastNOpponentStatLines));
            #endregion

            this.NumericalData = dataPoints.Select(x => x * info.ScaleFactor).ToList();
            this.DelimitedCategoricalData = String.Empty;
        }
    }
}
