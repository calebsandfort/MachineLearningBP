using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.DataPointMethods;

namespace MachineLearningBP.Entities.Sports.Nba
{
    public abstract class NbaFourFactorsExample<TExampleGenerationInfo>: SportNumbersOnlyExample<SportExampleGenerationInfo<NbaGame, NbaStatLine, NbaSeason, NbaTeam>, NbaGame, NbaStatLine, NbaSeason, NbaTeam>
        where TExampleGenerationInfo : SportExampleGenerationInfo<NbaGame, NbaStatLine, NbaSeason, NbaTeam>
    {
        public override void SetDataPoints(SportExampleGenerationInfo<NbaGame, NbaStatLine, NbaSeason, NbaTeam>  info)
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

            this.DataPoints = dataPoints.Select(x => x * info.ScaleFactor).ToList();
        }

        
    }
}
