using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public abstract class StatLineMinimum<TSample> : StatLine
        where TSample : Sample
    {
        [ForeignKey("SampleId")]
        public virtual TSample Sample { get; set; }
        public virtual int SampleId { get; set; }
    }
}
