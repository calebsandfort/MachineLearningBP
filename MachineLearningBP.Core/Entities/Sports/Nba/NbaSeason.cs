using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaSeasons")]
    public class NbaSeason : TimeGrouping, Season<NbaGame>
    {
        [ForeignKey("TimeGroupingId")]
        public virtual ICollection<NbaGame> Samples { get; set; }

        public DateTime? RollingWindowStart { get; set; }
    }
}
