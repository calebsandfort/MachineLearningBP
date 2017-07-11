using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports.Nfl.Dtos
{
    public class NflPlay
    {
        public int GameId { get; set; }
        public DateTime GameDate { get; set; }
        public String OffenseTeam { get; set; }
        public String DefenseTeam { get; set; }
        public int Down { get; set; }
        public Double ToGo { get; set; }
        public Double Yards { get; set; }
    }

    public sealed class NflPlayMap : CsvClassMap<NflPlay>
    {
        public NflPlayMap()
        {
            Map(m => m.GameId);
            Map(m => m.GameDate);
            Map(m => m.OffenseTeam);
            Map(m => m.DefenseTeam);
            Map(m => m.Down);
            Map(m => m.ToGo);
            Map(m => m.Yards).Default(0);
        }
    }
}
