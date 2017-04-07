﻿using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public interface SampleMinimum<TStatLine>
        where TStatLine : StatLine
    {
        //[ForeignKey("SampleId")]
        ICollection<TStatLine> StatLines { get; set; }
    }
}
