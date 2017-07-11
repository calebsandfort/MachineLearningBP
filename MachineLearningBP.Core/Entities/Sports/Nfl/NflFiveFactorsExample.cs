using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.DataPointMethods;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Nfl
{
    public abstract class NflFiveFactorsExample<TResult> : ExampleMinimum<NflStatLine, TResult, NflExampleGenerationInfo>
    {
        #region Properties
        [NotMapped]
        public Double YardsPerPlay { get; set; }
        [NotMapped]
        public Double YardsPerPlayOpponent { get; set; }

        [NotMapped]
        public Double SuccessRate { get; set; }
        [NotMapped]
        public Double SuccessRateOpponent { get; set; }

        [NotMapped]
        public Double FieldPosition { get; set; }
        [NotMapped]
        public Double FieldPositionOpponent { get; set; }

        [NotMapped]
        public Double PointsPerDriveInside40 { get; set; }
        [NotMapped]
        public Double PointsPerDriveInside40Opponent { get; set; }

        [NotMapped]
        public Double Turnovers { get; set; }
        [NotMapped]
        public Double TurnoversOpponent { get; set; }

        [NotMapped]
        public Double TurnoversForced { get; set; }
        [NotMapped]
        public Double TurnoversForcedOpponent { get; set; }
        #endregion

        public override void SetData(NflExampleGenerationInfo info)
        {
            this.YardsPerPlay = info.Team1LastNStatLines.Sum(x => x.TotalYards) / info.Team1LastNStatLines.Sum(x => x.TotalPlays);
            this.YardsPerPlayOpponent = info.Team2LastNStatLines.Sum(x => x.TotalYards) / info.Team2LastNStatLines.Sum(x => x.TotalPlays);

            this.SuccessRate = info.Team1LastNStatLines.Sum(x => x.TotalSuccessfulSuccessRatePlays) / info.Team1LastNStatLines.Sum(x => x.TotalSuccessRatePlays);
            this.SuccessRateOpponent = info.Team2LastNStatLines.Sum(x => x.TotalSuccessfulSuccessRatePlays) / info.Team2LastNStatLines.Sum(x => x.TotalSuccessRatePlays);

            this.FieldPosition = info.Team1LastNStatLines.Sum(x => x.TotalDrives * x.FieldPosition) / info.Team1LastNStatLines.Sum(x => x.TotalDrives);
            this.FieldPositionOpponent = info.Team2LastNStatLines.Sum(x => x.TotalDrives * x.FieldPosition) / info.Team2LastNStatLines.Sum(x => x.TotalDrives);

            this.PointsPerDriveInside40 = info.Team1LastNStatLines.Sum(x => x.PointsDrivesInside40) / info.Team1LastNStatLines.Sum(x => x.DrivesInside40);
            this.PointsPerDriveInside40Opponent = info.Team2LastNStatLines.Sum(x => x.PointsDrivesInside40) / info.Team2LastNStatLines.Sum(x => x.DrivesInside40);

            this.TurnoversForced = info.Team1LastNStatLines.Average(x => x.TurnoversForced);
            this.TurnoversForcedOpponent = info.Team2LastNStatLines.Average(x => x.TurnoversForced);
        }
    }
}
