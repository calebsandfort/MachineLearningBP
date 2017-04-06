using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Mlb
{
    [Table("MlbGames")]
    public class MlbGame : Game<MlbStatLine, MlbSeason>
    {
        public int CoversId { get; set; }
    }
}
