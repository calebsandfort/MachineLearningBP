using MachineLearningBP.CollectiveIntelligence.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace MachineLearningBP.Entities.Sports.Mlb
{
    [Table("MlbSeasons")]
    public class MlbSeason : TimeGrouping, Season<MlbGame>
    {
        [ForeignKey("TimeGroupingId")]
        public virtual ICollection<MlbGame> Samples { get; set; }

        public DateTime? RollingWindowStart { get; set; }
    }
}
