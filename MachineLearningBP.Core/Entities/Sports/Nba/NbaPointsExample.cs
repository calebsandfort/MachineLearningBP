using Abp.Timing;
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
    public class NbaPointsExample : NbaFourFactorsExample<Double>
    {
        public DateTime Date { get; set; }

        [ForeignKey("StatLineId")]
        public override NbaStatLine StatLine { get; set; }
        public override int StatLineId { get; set; }

        public override bool PythonIndexOnly
        {
            get { return true; }
        }

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

            this.NumericData = dataPoints.Select(x => x * info.ScaleFactor).ToList();
            this.Date = info.Game.Date;
        }

        public override void SetResult(NbaExampleGenerationInfo info)
        {
            this.Result = info.TeamStatLine1.Points;
        }
    }
}
