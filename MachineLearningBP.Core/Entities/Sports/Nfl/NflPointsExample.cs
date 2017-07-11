using Abp.Timing;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports.Nfl
{
    [Table("NflPointsExamples")]
    public class NflPointsExample : NflFiveFactorsExample<Double>
    {
        public DateTime Date { get; set; }

        [ForeignKey("StatLineId")]
        public override NflStatLine StatLine { get; set; }
        public override int StatLineId { get; set; }

        public override bool PythonIndexOnly
        {
            get { return true; }
        }

        public override void SetData(NflExampleGenerationInfo info)
        {
            base.SetData(info);

            List<Double> dataPoints = new List<double>();
            dataPoints.Add(this.YardsPerPlay - this.YardsPerPlayOpponent);
            dataPoints.Add(this.SuccessRate - this.SuccessRateOpponent);
            dataPoints.Add(this.FieldPosition - this.FieldPositionOpponent);
            dataPoints.Add(this.PointsPerDriveInside40);
            dataPoints.Add(this.TurnoversForced - this.TurnoversOpponent);

            this.NumericData = dataPoints.Select(x => x * info.ScaleFactor).ToList();
            this.Date = info.Game.Date;
        }

        public override void SetResult(NflExampleGenerationInfo info)
        {
            this.Result = info.TeamStatLine1.Points;
        }
    }
}
