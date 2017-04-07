using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearningBP.Entities.Sports.Mlb
{
    [Table("MlbGames")]
    public class MlbGame : Sample, Game<MlbSeason, MlbStatLine>
    {
        [ForeignKey("SampleId")]
        public virtual ICollection<MlbStatLine> StatLines { get; set; }

        [ForeignKey("TimeGroupingId")]
        public virtual MlbSeason TimeGrouping { get; set; }
        public virtual int TimeGroupingId { get; set; }

        public DateTime Date { get; set; }
        public int EspnIdentifier { get; set; }
        public bool Completed { get; set; }
        public double Total { get; set; }

        public int CoversId { get; set; }
    }
}
