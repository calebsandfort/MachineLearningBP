using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaGames")]
    public class NbaGame : Game<NbaStatLine, NbaSeason>
    {
        public Double Spread { get; set; }
    }
}
