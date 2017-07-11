
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Nfl
{
    [Table("NflStatLines")]
    public class NflStatLine : StatLine, SportStatLine<NflTeam, NflGame>
    {
        [ForeignKey("SampleId")]
        public virtual NflGame Sample { get; set; }
        public virtual int SampleId { get; set; }

        [ForeignKey("ParticipantId")]
        public virtual NflTeam Participant { get; set; }
        public virtual int ParticipantId { get; set; }

        public double Points { get; set; }
        public double KnnPoints { get; set; }
        public bool Home { get; set; }

        public Double TotalPlays { get; set; }
        public Double TotalYards { get; set; }
        public Double YardsPerPlay { get; set; }

        public Double TotalDrives { get; set; }
        public Double FieldPosition { get; set; }

        public Double DrivesInside40 { get; set; }
        public Double PointsDrivesInside40 { get; set; }
        public Double PointsPerDriveInside40 { get; set; }

        public Double Turnovers { get; set; }
        public Double TurnoversForced { get; set; }
        public Double TurnoverMargin { get; set; }

        public Double TotalSuccessRatePlays { get; set; }
        public Double TotalSuccessfulSuccessRatePlays { get; set; }
        public Double SuccessRate { get; set; }

        public NflStatLine()
        {
            this.TotalSuccessRatePlays = 0;
            this.TotalSuccessfulSuccessRatePlays = 0;
        }
    }
}
