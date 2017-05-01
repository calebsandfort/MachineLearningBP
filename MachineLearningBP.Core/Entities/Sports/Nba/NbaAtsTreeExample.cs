using Abp.Timing;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaAtsTreeExamples")]
    public class NbaAtsTreeExample : NbaFourFactorsExample<Double>
    {
        public DateTime Date { get; set; }

        [ForeignKey("StatLineId")]
        public override NbaStatLine StatLine { get; set; }
        public override int StatLineId { get; set; }

        public override bool PythonIndexOnly
        {
            get { return false; }
        }

        public override void SetData(NbaExampleGenerationInfo info)
        {
            base.SetData(info);

            List<Double> numericalData = new List<double>();
            List<String> categoricalData = new List<String>();

            #region Shooting
            //categoricalData.Add(ShootingOffense > ShootingOpponentDefense ? "yes" : "no");
            //categoricalData.Add(ShootingDefense < ShootingOpponentOffense ? "yes" : "no");
            numericalData.Add(ShootingOffense - ShootingOpponentDefense);
            numericalData.Add(ShootingOpponentOffense - ShootingDefense);
            #endregion

            #region Turnovers
            //categoricalData.Add(TurnoversOffense < TurnoversOpponentDefense ? "yes" : "no");
            //categoricalData.Add(TurnoversDefense > TurnoversOpponentOffense ? "yes" : "no");
            numericalData.Add(TurnoversOffense - TurnoversOpponentDefense);
            numericalData.Add(TurnoversDefense - TurnoversOpponentOffense);
            #endregion

            #region Rebounding
            //categoricalData.Add((ReboundingOffense + ReboundingDefense) > (ReboundingOpponentOffense + ReboundingOpponentDefense) ? "yes" : "no");
            numericalData.Add(ReboundingOffense + ReboundingDefense);
            numericalData.Add(ReboundingOpponentOffense + ReboundingOpponentDefense);
            #endregion

            #region Free Throws
            //categoricalData.Add(FreeThrowsOffense > FreeThrowsOpponentDefense ? "yes" : "no");
            //categoricalData.Add(FreeThrowsDefense < FreeThrowsOpponentOffense ? "yes" : "no");
            numericalData.Add(FreeThrowsOffense - FreeThrowsOpponentDefense);
            numericalData.Add(FreeThrowsDefense - FreeThrowsOpponentOffense);
            #endregion

            #region Categorical
            categoricalData.Add(info.TeamStatLine1.Home ? "yes" : "no");
            categoricalData.Add(info.TeamStatLine1.TwoInTwoDays ? "yes" : "no");
            categoricalData.Add(info.TeamStatLine2.TwoInTwoDays ? "yes" : "no");
            #endregion

            this.OrdinalData = numericalData.Select(x => x * info.ScaleFactor).ToList();
            this.NominalData = categoricalData;
            this.Date = info.Game.Date;
        }

        public override void SetResult(NbaExampleGenerationInfo info)
        {
            Double spread = info.TeamStatLine1.Home ? info.Game.Spread : -info.Game.Spread;
            Double pointDifferential = info.TeamStatLine1.Points - info.TeamStatLine2.Points;
            //Double spreadDifferential = spread + pointDifferential;

            //this.Result = Bucket.GetBucket(pointDifferential).ToString();
            this.Result = info.TeamStatLine1.Points;
        }
    }
}
