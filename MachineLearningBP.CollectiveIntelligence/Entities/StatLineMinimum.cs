using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public interface StatLineMinimum<TSample>
        where TSample : Sample
    {
        //[ForeignKey("SampleId")]
        TSample Sample { get; set; }
        int SampleId { get; set; }
    }
}
