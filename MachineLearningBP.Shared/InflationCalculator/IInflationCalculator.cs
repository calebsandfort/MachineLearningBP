using Abp.Dependency;
using System;
using System.Collections.Generic;

namespace MachineLearningBP.Shared.InflationCalculator
{
    public interface IInflationCalculator : ISingletonDependency
    {
        void GenCpiDictionary();
        double CalculateCpi(double val, DateTime from, DateTime to);

        void GenTheaterCountDictionary(List<KeyValuePair<int, double>> theaterCounts);
        double CalculateTheaterCount(double val, DateTime from, DateTime to);
    }
}
