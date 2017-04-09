using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaPointsExamples")]
    public class NbaPointsExample : NbaFourFactorsExample
    {
        [ForeignKey("StatLineId")]
        public override NbaStatLine StatLine { get; set; }
        public override int StatLineId { get; set; }

        public override void SetData(NbaExampleGenerationInfo info)
        {
            base.SetData(info);

            List<Double> dataPoints = new List<double>();

            #region Shooting
            dataPoints.Add(ShootingOffense);
            dataPoints.Add(ShootingDefense);
            dataPoints.Add(ShootingOpponentOffense);
            dataPoints.Add(ShootingOpponentDefense);
            #endregion

            #region Turnovers
            dataPoints.Add(TurnoversOffense);
            dataPoints.Add(TurnoversDefense);
            dataPoints.Add(TurnoversOpponentOffense);
            dataPoints.Add(TurnoversOpponentDefense);
            #endregion

            #region Rebounding
            dataPoints.Add(ReboundingOffense);
            dataPoints.Add(ReboundingDefense);
            dataPoints.Add(ReboundingOpponentOffense);
            dataPoints.Add(ReboundingOpponentDefense);
            #endregion

            #region Free Throws
            dataPoints.Add(FreeThrowsOffense);
            dataPoints.Add(FreeThrowsDefense);
            dataPoints.Add(FreeThrowsOpponentOffense);
            dataPoints.Add(FreeThrowsOpponentDefense);
            #endregion

            this.NumericalData = dataPoints.Select(x => x * info.ScaleFactor).ToList();
            this.DelimitedCategoricalData = String.Empty;
        }

        public override void SetResult(NbaExampleGenerationInfo info)
        {
            this.Result = info.TeamStatLine1.Points;
        }
    }
}
