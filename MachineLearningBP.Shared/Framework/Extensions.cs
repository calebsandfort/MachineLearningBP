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

        public static int ToMonthInt(String val)
        {
            int month = 0;

            switch (val)
            {
                case "Jan":
                    month = 1;
                    break;
                case "Feb":
                    month = 2;
                    break;
                case "Mar":
                    month = 3;
                    break;
                case "Apr":
                    month = 4;
                    break;
                case "May":
                    month = 5;
                    break;
                case "June":
                    month = 6;
                    break;
                case "July":
                    month = 7;
                    break;
                case "Aug":
                    month = 8;
                    break;
                case "Sep":
                    month = 9;
                    break;
                case "Oct":
                    month = 10;
                    break;
                case "Nov":
                    month = 11;
                    break;
                case "Dec":
                    month = 12;
                    break;
            }

            return month;
        }
    }
}
