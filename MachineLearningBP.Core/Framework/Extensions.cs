using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MachineLearningBP.Framework
{
    public static class Extensions
    {
        private static readonly Regex _regex = new Regex(@"[?|&]([\w\.]+)=([^?|^&]+)", RegexOptions.Compiled);
        static Regex moneyRegEx = new Regex(@"\b(\d+)\s*(thousand|million)\b", RegexOptions.Compiled);

        public static Double ParseMoney(this String val)
        {
            if (val == "N/A") return 0;

            Double number = 0;
            if (Double.TryParse(val, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, new CultureInfo("en-US"), out number) && number > 0) return number;

            MatchCollection mc = moneyRegEx.Matches(val.Trim());

            try
            {
                return Double.Parse(mc[0].Groups[1].Value) * mc[0].Groups[2].Value.WordToNumber();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static Double WordToNumber(this String word)
        {
            Double number = 0;

            switch (word)
            {
                case "million":
                    number = 1000000;
                    break;
                case "thousand":
                    number = 1000;
                    break;
            }

            return number;
        }

        public static List<Double> SplitDoubles(this String val)
        {
            return val.Split("-".ToCharArray()).Select(x => Double.Parse(x)).ToList();
        }

        public static Double ToDouble(this String val)
        {
            return Double.Parse(val);
        }

        public static IReadOnlyDictionary<string, string> ParseQueryString(this Uri uri)
        {
            var match = _regex.Match(uri.PathAndQuery);
            var paramaters = new Dictionary<string, string>();
            while (match.Success)
            {
                paramaters.Add(match.Groups[1].Value, match.Groups[2].Value);
                match = match.NextMatch();
            }
            return paramaters;
        }

        public static String ScrapifyNode(this HtmlNode node)
        {
            return node.InnerText.Replace("(Estimate)", String.Empty);
        }
    }
}
