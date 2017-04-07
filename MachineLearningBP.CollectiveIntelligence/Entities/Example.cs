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
        public void SetFields(TExampleGenerationInfo info)
        {
            this.SetData(info);
            this.SetResult(info);
        }

        public abstract void SetData(TExampleGenerationInfo info);
        public abstract void SetResult(TExampleGenerationInfo info);
    }
}
