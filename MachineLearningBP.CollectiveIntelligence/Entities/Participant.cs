﻿using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public abstract class Participant : Entity<int>
    {
        public String Name { get; set; }
    }
}
