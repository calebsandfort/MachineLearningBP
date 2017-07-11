using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports.Nfl
{
    public class NflWeek
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool Within(DateTime date)
        {
            return (date >= this.Start && date < this.End);
        }
    }
}
