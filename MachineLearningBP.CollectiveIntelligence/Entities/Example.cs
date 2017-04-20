using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Entities
{
    public abstract class Example<TStatLine, TResult> : Entity<int>
        where TStatLine : StatLine
    {
        //[ForeignKey("StatLineId")]
        public abstract TStatLine StatLine { get; set; }
        public abstract int StatLineId { get; set; }

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
                    this.DelimitedNumericalData = String.Join(":", value.Select(x => String.Format("{0:N4}", x)));
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

        [NotMapped]
        private List<String> combinedData = new List<string>();
        [NotMapped]
        public List<String> CombinedData
        {
            get
            {
                if(combinedData.Count == 0)
                {
                    if (this.NumericalData.Count > 0)
                        combinedData.AddRange(this.NumericalData.Select(x => x.ToString("N2")));

                    if (this.CategoricalData.Count > 0)
                        combinedData.AddRange(this.CategoricalData);
                }

                return this.combinedData;
            }
        }
    }
}
