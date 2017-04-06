﻿using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public abstract class Example<TExampleGenerationInfo> : Entity<int>
        where TExampleGenerationInfo : ExampleGenerationInfo
    {
        public abstract void SetFields(TExampleGenerationInfo info);
    }
}
