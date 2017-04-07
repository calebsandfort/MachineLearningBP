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
        public String DelimitedPoints { get; set; }

        [NotMapped]
        public List<Double> Data
        {
            get
            {
                if (String.IsNullOrEmpty(this.DelimitedPoints))
                    return new List<double>();
                else
                    return this.DelimitedPoints.Split(":".ToCharArray()).Select(x => Double.Parse(x)).ToList();
            }
            set
            {
                if (value.Count == 0)
                    this.DelimitedPoints = String.Empty;
                else
                    this.DelimitedPoints = String.Join(":", value);
            }
        }
    }
}
