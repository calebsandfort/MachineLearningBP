using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Framework
{
    public static class Extensions
    {
        public static List<Double> SplitDoubles(this String val)
        {
            return val.Split("-".ToCharArray()).Select(x => Double.Parse(x)).ToList();
        }

        public static Double ToDouble(this String val)
        {
            return Double.Parse(val);
        }
    }
}
