using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.StatLines
{
    public abstract class StatLine : Entity<int>
    {
        public Double Points { get; set; }
        public bool Home { get; set; }
    }
}
