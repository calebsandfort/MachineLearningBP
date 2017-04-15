using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MachineLearningBP.Shared.Framework
{
    public static class Extensions
    {
        static Regex filePathRegEx = new Regex(@"[^\\]+(?=\.cs$)");

        public static String CombineFileAndMember(String filePath, String memberName)
        {
            MatchCollection mc = filePathRegEx.Matches(filePath.Trim());
            return $"{mc[0].Groups[0].Value}.{memberName}";
        }
    }
}
