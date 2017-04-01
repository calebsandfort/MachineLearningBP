using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Games
{
    public abstract class Game : Entity<int>
    {
        public DateTime Date { get; set; }
        public int EspnIdentifier { get; set; }
        public bool Completed { get; set; }
        public Double Spread { get; set; }
        public Double Total { get; set; }
    }
}
