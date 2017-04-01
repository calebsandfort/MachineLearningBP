using MachineLearningBP.Games;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Seasons
{
    [Table("NbaSeasons")]
    public class NbaSeason : Season<NbaGame>
    {
        [ForeignKey("SeasonId")]
        public override ICollection<NbaGame> Games { get; set; }
    }
}
