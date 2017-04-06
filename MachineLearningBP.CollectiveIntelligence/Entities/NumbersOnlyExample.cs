using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public abstract class NumbersOnlyExample<TExampleGenerationInfo> : Example<TExampleGenerationInfo>
        where TExampleGenerationInfo : ExampleGenerationInfo
    {
        public Double Result { get; set; }
        public String DelimitedDataPoints { get; set; }

        [NotMapped]
        public List<Double> DataPoints
        {
            get
            {
                if (String.IsNullOrEmpty(this.DelimitedDataPoints))
                    return new List<double>();
                else
                    return this.DelimitedDataPoints.Split(":".ToCharArray()).Select(x => Double.Parse(x)).ToList();
            }
            set
            {
                if (value.Count == 0)
                    this.DelimitedDataPoints = String.Empty;
                else
                    this.DelimitedDataPoints = String.Join(":", value);
            }
        }

        public override void SetFields(TExampleGenerationInfo info)
        {
            this.SetDataPoints(info);
            this.SetResult(info);
        }

        public abstract void SetDataPoints(TExampleGenerationInfo info);
        public abstract void SetResult(TExampleGenerationInfo info);
    }
}
