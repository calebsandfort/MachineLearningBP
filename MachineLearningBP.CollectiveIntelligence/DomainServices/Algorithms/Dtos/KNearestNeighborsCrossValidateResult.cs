using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class KNearestNeighborsCrossValidateResult : IRecordContainer
    {
        public KNearestNeighborsGuessMethods GuessMethod { get; set; }
        public KNearestNeighborsWeightMethods WeightMethod { get; set; }
        public int Trials { get; set; }
        public int K { get; set; }
        public Double Result { get; set; }

        public List<string> ColumnHeaders
        {
            get
            {
                List<String> columnHeaders = new List<string>();

                columnHeaders.Add("Guess Method");
                columnHeaders.Add("Weight Method");
                columnHeaders.Add("Trials");
                columnHeaders.Add("K");
                columnHeaders.Add("Result");

                return columnHeaders;
            }
        }

        public List<string> ColumnValues
        {
            get
            {
                List<String> columnValues = new List<string>();

                columnValues.Add(this.GuessMethod.ToString());
                columnValues.Add(this.GuessMethod == KNearestNeighborsGuessMethods.WeightedKnn ? this.WeightMethod.ToString() : String.Empty);
                columnValues.Add(this.Trials.ToString());
                columnValues.Add(this.K.ToString());
                columnValues.Add(this.Result.ToString("N4"));

                return columnValues;
            }
        }

        public KNearestNeighborsCrossValidateResult(
            KNearestNeighborsGuessMethods guessMethod,
            KNearestNeighborsWeightMethods weightMethod,
            int trials,
            int k,
            double result)
        {
            this.GuessMethod = guessMethod;
            this.WeightMethod = weightMethod;
            this.Trials = trials;
            this.K = k;
            this.Result = result;
        }
    }
}
