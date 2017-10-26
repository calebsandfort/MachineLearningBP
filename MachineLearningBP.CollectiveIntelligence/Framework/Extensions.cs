using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.Framework
{
    public static class Extensions
    {
        public static String ToRDouble(this Double val, NumberFormatInfo nfi, params Double[] naValues)
        {
            if (naValues != null && naValues.Contains(val)) return "NA";

            return val.ToString("N2", nfi);
        }

        public static String ToRInt(this Double val, NumberFormatInfo nfi, params Double[] naValues)
        {
            if (naValues != null && naValues.Contains(val)) return "NA";

            return val.ToString("N0", nfi);
        }

        public static String ToRString(this String val, params String[] naValues)
        {
            if (naValues != null && naValues.Contains(val)) return "NA";

            return $"\"{val}\"";
        }

        public static String ToRBool(this bool val)
        {
            return val ? "1" : "0";
        }
    }
}
