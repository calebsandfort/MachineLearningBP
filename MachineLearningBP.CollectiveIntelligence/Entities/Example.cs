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
        public String DelimitedOrdinalData { get; set; }

        [NotMapped]
        public List<Double> OrdinalData
        {
            get
            {
                if (String.IsNullOrEmpty(this.DelimitedOrdinalData))
                    return new List<double>();
                else
                    return this.DelimitedOrdinalData.Split(":".ToCharArray()).Select(x => Double.Parse(x)).ToList();
            }
            set
            {
                if (value.Count == 0)
                    this.DelimitedOrdinalData = String.Empty;
                else
                    this.DelimitedOrdinalData = String.Join(":", value.Select(x => String.Format("{0:N4}", x)));
            }
        }

        public String DelimitedNominalData { get; set; }

        [NotMapped]
        public List<String> NominalData
        {
            get
            {
                if (String.IsNullOrEmpty(this.DelimitedNominalData))
                    return new List<String>();
                else
                    return this.DelimitedNominalData.Split(":".ToCharArray()).ToList();
            }
            set
            {
                if (value.Count == 0)
                    this.DelimitedNominalData = String.Empty;
                else
                    this.DelimitedNominalData = String.Join(":", value);
            }
        }

        public String DelimitedBinaryData { get; set; }

        [NotMapped]
        public List<bool> BinaryData
        {
            get
            {
                if (String.IsNullOrEmpty(this.DelimitedBinaryData))
                    return new List<bool>();
                else
                    return this.DelimitedBinaryData.Split(":".ToCharArray()).Select(x => Boolean.Parse(x)).ToList();
            }
            set
            {
                if (value.Count == 0)
                    this.DelimitedBinaryData = String.Empty;
                else
                    this.DelimitedBinaryData = String.Join(":", value);
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
                    if (this.OrdinalData.Count > 0)
                        combinedData.AddRange(this.OrdinalData.Select(x => x.ToString("N2")));

                    if (this.NominalData.Count > 0)
                        combinedData.AddRange(this.NominalData);

                    if (this.BinaryData.Count > 0)
                        combinedData.AddRange(this.BinaryData.Select(x => x.ToString()));
                }

                return this.combinedData;
            }
        }
    }
}
