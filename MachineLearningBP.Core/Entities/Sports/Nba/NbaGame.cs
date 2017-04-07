using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaGames")]
    public class NbaGame : Sample, Game<NbaSeason, NbaStatLine>
    {
        [ForeignKey("SampleId")]
        public virtual ICollection<NbaStatLine> StatLines { get; set; }

        [ForeignKey("TimeGroupingId")]
        public virtual NbaSeason TimeGrouping { get; set; }
        public virtual int TimeGroupingId { get; set; }

        public DateTime Date { get; set; }
        public int EspnIdentifier { get; set; }
        public bool Completed { get; set; }
        public double Total { get; set; }

        public Double Spread { get; set; }
    }
}
