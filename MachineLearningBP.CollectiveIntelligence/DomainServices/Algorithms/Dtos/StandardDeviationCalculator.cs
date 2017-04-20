using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class StandardDeviationCalculator
    {
        public Double Mean { get; set; }
        public Double StdDev { get; set; }

        public StandardDeviationCalculator() { }

        public StandardDeviationCalculator(IEnumerable<Double> values)
        {
            this.Mean = values.Average();
            double sum = values.Sum(d => Math.Pow(d - this.Mean, 2));
            this.StdDev = Math.Sqrt((sum) / values.Count() - 1);
        }

        public Double CalculateZScore(Double x)
        {
            return (x - this.Mean) / this.StdDev;
        }
    }
}
