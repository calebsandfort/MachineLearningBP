using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public abstract class Example<TResult, TExampleGenerationInfo> : Entity<int>
        where TExampleGenerationInfo : ExampleGenerationInfo
    {
        public TResult Result { get; set; }
        public String DelimitedNumericalData { get; set; }

        [NotMapped]
        public List<Double> NumericalData
        {
            get
            {
                if (String.IsNullOrEmpty(this.DelimitedNumericalData))
                    return new List<double>();
                else
                    return this.DelimitedNumericalData.Split(":".ToCharArray()).Select(x => Double.Parse(x)).ToList();
            }
            set
            {
                if (value.Count == 0)
                    this.DelimitedNumericalData = String.Empty;
                else
                    this.DelimitedNumericalData = String.Join(":", value);
            }
        }

        public String DelimitedCategoricalData { get; set; }

        [NotMapped]
        public List<String> CategoricalData
        {
            get
            {
                if (String.IsNullOrEmpty(this.DelimitedCategoricalData))
                    return new List<String>();
                else
                    return this.DelimitedCategoricalData.Split(":".ToCharArray()).ToList();
            }
            set
            {
                if (value.Count == 0)
                    this.DelimitedCategoricalData = String.Empty;
                else
                    this.DelimitedCategoricalData = String.Join(":", value);
            }
        }

        public void SetFields(TExampleGenerationInfo info)
        {
            this.SetData(info);
            this.SetResult(info);
        }

        public abstract void SetData(TExampleGenerationInfo info);
        public abstract void SetResult(TExampleGenerationInfo info);
    }
}
