using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public interface IExample<TResult>
    {
        TResult Result { get; set; }

        String DelimitedNumericalData { get; set; }
        List<Double> NumericalData { get; set; }

        String DelimitedCategoricalData { get; set; }
        List<String> CategoricalData { get; set; }
    }
}
