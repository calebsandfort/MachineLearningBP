using MachineLearningBP.Seasons;
using MachineLearningBP.StatLines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Games
{
    [Table("NbaGames")]
    public class NbaGame : Game
    {
        [ForeignKey("SeasonId")]
        public virtual NbaSeason Season { get; set; }
        public virtual int SeasonId { get; set; }

        [ForeignKey("GameId")]
        public virtual ICollection<NbaStatLine> StatLines { get; set; }
    }
}
