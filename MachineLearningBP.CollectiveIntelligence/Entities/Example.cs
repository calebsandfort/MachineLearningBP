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
        public abstract bool PythonIndexOnly { get; }

        #region Numeric
        public String DelimitedNumericData { get; set; }

        [NotMapped]
        public List<Double> NumericData
        {
            get
            {
                if (String.IsNullOrEmpty(this.DelimitedNumericData))
                    return new List<double>();
                else
                    return this.DelimitedNumericData.Split(":".ToCharArray()).Select(x => Double.Parse(x)).ToList();
            }
            set
            {
                if (value.Count == 0)
                    this.DelimitedNumericData = String.Empty;
                else
                    this.DelimitedNumericData = String.Join(":", value.Select(x => String.Format("{0:N4}", x)));
            }
        }
        #endregion

        #region Ordinal
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
        #endregion

        #region Nominal
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
        #endregion

        #region AsymmBinary
        public String DelimitedAsymmBinaryData { get; set; }

        [NotMapped]
        public List<bool> AsymmBinaryData
        {
            get
            {
                if (String.IsNullOrEmpty(this.DelimitedAsymmBinaryData))
                    return new List<bool>();
                else
                    return this.DelimitedAsymmBinaryData.Split(":".ToCharArray()).Select(x => Boolean.Parse(x)).ToList();
            }
            set
            {
                if (value.Count == 0)
                    this.DelimitedAsymmBinaryData = String.Empty;
                else
                    this.DelimitedAsymmBinaryData = String.Join(":", value);
            }
        }
        #endregion

        #region SymmBinary
        public String DelimitedSymmBinaryData { get; set; }

        [NotMapped]
        public List<bool> SymmBinaryData
        {
            get
            {
                if (String.IsNullOrEmpty(this.DelimitedSymmBinaryData))
                    return new List<bool>();
                else
                    return this.DelimitedSymmBinaryData.Split(":".ToCharArray()).Select(x => Boolean.Parse(x)).ToList();
            }
            set
            {
                if (value.Count == 0)
                    this.DelimitedSymmBinaryData = String.Empty;
                else
                    this.DelimitedSymmBinaryData = String.Join(":", value);
            }
        }
        #endregion

        #region Combined
        [NotMapped]
        private List<String> combinedData = new List<string>();
        [NotMapped]
        public List<String> CombinedData
        {
            get
            {
                if (combinedData.Count == 0)
                {
                    if (this.NumericData.Count > 0)
                        combinedData.AddRange(this.NumericData.Select(x => x.ToString("N2")));

                    if (this.OrdinalData.Count > 0)
                        combinedData.AddRange(this.OrdinalData.Select(x => x.ToString("N2")));

                    if (this.NominalData.Count > 0)
                        combinedData.AddRange(this.NominalData);

                    if (this.AsymmBinaryData.Count > 0)
                        combinedData.AddRange(this.AsymmBinaryData.Select(x => x.ToString()));

                    if (this.SymmBinaryData.Count > 0)
                        combinedData.AddRange(this.SymmBinaryData.Select(x => x.ToString()));
                }

                return this.combinedData;
            }
        } 
        #endregion
    }
}
