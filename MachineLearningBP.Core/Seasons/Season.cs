using Abp.Domain.Entities;
using MachineLearningBP.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Seasons
{
    public abstract class Season<TGame> : Entity<int>
        where TGame : Game
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime? RollingWindowStart { get; set; }

        public abstract ICollection<TGame> Games { get; set; }

        public bool Within(DateTime date)
        {
            return (date >= this.Start && date <= this.End);
        }
    }
}
