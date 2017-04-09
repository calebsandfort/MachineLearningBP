using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.DataPointMethods;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Nba
{
    public abstract class NbaFourFactorsExample : ExampleMinimum<NbaStatLine, Double, NbaExampleGenerationInfo>
    {
        #region Properties
        [NotMapped]
        public Double ShootingOffense { get; set; }
        [NotMapped]
        public Double ShootingDefense { get; set; }
        [NotMapped]
        public Double ShootingOpponentOffense { get; set; }
        [NotMapped]
        public Double ShootingOpponentDefense { get; set; }

        [NotMapped]
        public Double TurnoversOffense { get; set; }
        [NotMapped]
        public Double TurnoversDefense { get; set; }
        [NotMapped]
        public Double TurnoversOpponentOffense { get; set; }
        [NotMapped]
        public Double TurnoversOpponentDefense { get; set; }

        [NotMapped]
        public Double ReboundingOffense { get; set; }
        [NotMapped]
        public Double ReboundingDefense { get; set; }
        [NotMapped]
        public Double ReboundingOpponentOffense { get; set; }
        [NotMapped]
        public Double ReboundingOpponentDefense { get; set; }

        [NotMapped]
        public Double FreeThrowsOffense { get; set; }
        [NotMapped]
        public Double FreeThrowsDefense { get; set; }
        [NotMapped]
        public Double FreeThrowsOpponentOffense { get; set; }
        [NotMapped]
        public Double FreeThrowsOpponentDefense { get; set; } 
        #endregion

        public override void SetData(NbaExampleGenerationInfo info)
        {
            #region Shooting
            ShootingOffense = NbaMethods.CalculateShooting(info.Team1LastNStatLines);
            ShootingDefense = NbaMethods.CalculateShooting(info.Team1LastNOpponentStatLines);
            ShootingOpponentOffense = NbaMethods.CalculateShooting(info.Team2LastNStatLines);
            ShootingOpponentDefense = NbaMethods.CalculateShooting(info.Team2LastNOpponentStatLines);
            #endregion

            #region Turnovers
            TurnoversOffense = NbaMethods.CalculateTurnovers(info.Team1LastNStatLines);
            TurnoversDefense = NbaMethods.CalculateTurnovers(info.Team1LastNOpponentStatLines);
            TurnoversOpponentOffense = NbaMethods.CalculateTurnovers(info.Team2LastNStatLines);
            TurnoversOpponentDefense = NbaMethods.CalculateTurnovers(info.Team2LastNOpponentStatLines);
            #endregion

            #region Rebounding
            ReboundingOffense = NbaMethods.CalculateRebounding(info.Team1LastNStatLines, info.Team1LastNOpponentStatLines);
            ReboundingDefense = NbaMethods.CalculateRebounding(info.Team1LastNStatLines, info.Team1LastNOpponentStatLines);
            ReboundingOpponentOffense = NbaMethods.CalculateRebounding(info.Team2LastNStatLines, info.Team2LastNOpponentStatLines);
            ReboundingOpponentDefense = NbaMethods.CalculateRebounding(info.Team2LastNStatLines, info.Team2LastNOpponentStatLines);
            #endregion

            #region Free Throws
            FreeThrowsOffense = NbaMethods.CalculateFreeThrows(info.Team1LastNStatLines);
            FreeThrowsDefense = NbaMethods.CalculateFreeThrows(info.Team1LastNOpponentStatLines);
            FreeThrowsOpponentOffense = NbaMethods.CalculateFreeThrows(info.Team2LastNStatLines);
            FreeThrowsOpponentDefense = NbaMethods.CalculateFreeThrows(info.Team2LastNOpponentStatLines);
            #endregion
        }
    }
}
