
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Mlb
{
    [Table("MlbStatLines")]
    public class MlbStatLine : SportStatLine<MlbStatLine, MlbSeason>
    {
        public Double Moneyline { get; set; }
        public Double InningsPitched { get; set; }
        public Double AtBats { get; set; }
        public Double Walks { get; set; }
        public Double Hits { get; set; }
        public Double HitByPitch { get; set; }
        public Double SacrificeFlies { get; set; }
    }
}
