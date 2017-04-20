using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Configuration;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Shared.GuerillaTimer;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms
{
    public class DecisionTreeDomainService<TExample, TStatLine, TResult> : BaseDomainService, IDecisionTreeDomainService<TExample, TStatLine, TResult>
        where TExample : Example<TStatLine, TResult>, new()
        where TStatLine : StatLine
    {
        #region Constructor
        public DecisionTreeDomainService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager) : base(sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
        {
        }
        #endregion

        #region BuildTree
        public DecisionNode BuildTree(List<TExample> rows, Func<List<TExample>, double> scoref, int depth = 0)
        {
            //this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"depth: {depth}"));

            if (rows.Count == 0) return new DecisionNode();

            Double current_score = scoref(rows);

            //Set up some variables to track the best criteria
            Double p = 0.0;
            Double gain = 0.0;
            List<String> column_values = new List<string>();

            Double best_gain = 0.0;
            ValueIndexPair<String> best_criteria = null;
            List<TExample> best_set1 = new List<TExample>();
            List<TExample> best_set2 = new List<TExample>();

            List<TExample> temp_set1 = new List<TExample>();
            List<TExample> temp_set2 = new List<TExample>();

            int column_count = rows[0].CombinedData.Count;
            for (int col = 0; col < column_count; col++)
            {
                //Generate the list of different values in this column
                column_values = new List<string>();
                foreach (TExample row in rows)
                {
                    if (!column_values.Contains(row.CombinedData[col]))
                    {
                        column_values.Add(row.CombinedData[col]);
                    }
                }

                //Now try dividing the rows up for each value in this column
                foreach (String value in column_values)
                {
                    temp_set1 = new List<TExample>();
                    temp_set2 = new List<TExample>();
                    DivideSet(rows, out temp_set1, out temp_set2, col, value);

                    //Information Gain
                    p = (double)temp_set1.Count / (double)rows.Count;
                    gain = current_score - (p * scoref(temp_set1)) - ((1 - p) * scoref(temp_set2));
                    if (gain > best_gain && temp_set1.Count > 0 && temp_set2.Count > 0)
                    {
                        best_gain = gain;
                        best_criteria = new ValueIndexPair<string>(value, col);
                        best_set1 = new List<TExample>(temp_set1);
                        best_set2 = new List<TExample>(temp_set2);
                    }
                }
            }

            DecisionNode tree = new DecisionNode();

            //this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"best_gain: {best_gain:N2}"));

            //Create the sub branches
            if (best_gain > 0)
            {
                tree.col = best_criteria.Index;
                tree.value = best_criteria.Value;
                tree.tb = BuildTree(best_set1, scoref, depth + 1);
                tree.fb = BuildTree(best_set2, scoref, depth + 1);
            }
            else
            {
                tree.results = UniqueCounts(rows);
            }

            return tree;
        }
        #endregion

        #region Prune
        public void Prune(DecisionNode tree, Func<List<TExample>, Double> scoref, Double mingain = .1)
        {
            try
            {
                //If the branches aren't leaves, then prune them
                if (tree.tb != null && tree.tb.results == null)
                    Prune(tree.tb, scoref, mingain);
                if (tree.fb != null && tree.fb.results == null)
                    Prune(tree.fb, scoref, mingain);

                //If both the subbranches are now leaves, see if they should be merged
                if (tree.tb != null && tree.tb.results != null && tree.fb != null && tree.fb.results != null)
                {
                    //Build a combined dataset
                    List<TExample> tb = new List<TExample>();
                    List<TExample> fb = new List<TExample>();

                    foreach (TreeResult treeResult in tree.tb.results)
                    {
                        for (int i = 0; i < treeResult.count; i++)
                        {
                            TExample temp = new TExample();
                            temp.Result = (TResult)ToObject(treeResult.value);
                            tb.Add(temp);
                        }
                    }

                    foreach (TreeResult treeResult in tree.fb.results)
                    {
                        for (int i = 0; i < treeResult.count; i++)
                        {
                            TExample temp = new TExample();
                            temp.Result = (TResult)ToObject(treeResult.value);
                            fb.Add(temp);
                        }
                    }

                    List<TExample> merged = new List<TExample>();
                    merged.AddRange(tb);
                    merged.AddRange(fb);

                    //Test the reduction in entropy
                    double delta = scoref(merged) - (scoref(tb) + scoref(fb) / 2);
                    //this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"delta {delta:N2}"));

                    if (delta < mingain)
                    {
                        tree.tb = null;
                        tree.fb = null;
                        tree.results = UniqueCounts(merged);
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }
        #endregion

        #region Classify
        public List<TreeResult> Classify(TExample observation, DecisionNode tree)
        {
            if(tree.results != null)
            {
                return tree.results;
            }
            else
            {
                String v = observation.CombinedData[tree.col];
                DecisionNode branch;

                Double doubleV = 0.0;
                if (Double.TryParse(v, out doubleV))
                {
                    if (doubleV > Double.Parse(tree.value)) branch = tree.tb;
                    else branch = tree.fb;
                }
                else
                {
                    if (v == tree.value) branch = tree.tb;
                    else branch = tree.fb;
                }

                return Classify(observation, branch);
            }
        } 
        #endregion

        #region DivideSet
        public void DivideSet(List<TExample> rows, out List<TExample> set1, out List<TExample> set2, int column, String value)
        {
            Func<TExample, bool> split_function = (t) =>
            {
                Double doubleValue = 0.0;
                if (Double.TryParse(value, out doubleValue))
                    return Double.Parse(t.CombinedData[column]) >= doubleValue;

                return t.CombinedData[column] == value;
            };

            set1 = rows.Where(split_function).ToList();
            set2 = rows.Where(x => !split_function(x)).ToList();
        }
        #endregion

        #region UniqueCounts
        public List<TreeResult> UniqueCounts(List<TExample> rows)
        {
            List<TreeResult> results = new List<TreeResult>();

            foreach(TExample row in rows)
            {
                if(results.Any(x => x.value == row.Result.ToString()))
                {
                    results.First(x => x.value == row.Result.ToString()).count += 1;
                }
                else
                {
                    results.Add(new TreeResult() { value = row.Result.ToString(), count = 1 });
                }
            }

            return results;
        }
        #endregion

        #region GiniImpurity
        public Double GiniImpurity(List<TExample> rows)
        {
            Double total = rows.Count;
            List<TreeResult> counts = UniqueCounts(rows);
            Double imp = 0;
            Double p1 = 0;
            Double p2 = 0;

            foreach (TreeResult k1 in counts)
            {
                p1 = k1.count / total;
                foreach (TreeResult k2 in counts)
                {
                    if (k1.value == k2.value) continue;

                    p2 = k2.count / total;
                    imp += p1 * p2;
                }
            }

            return imp;
        }
        #endregion

        #region Entropy
        public Double Entropy(List<TExample> rows)
        {
            Double total = rows.Count;
            List<TreeResult> results = UniqueCounts(rows);
            Double ent = 0;
            Double p = 0;

            foreach (TreeResult r in results)
            {
                p = r.count / total;
                ent -= p * Math.Log(p, 2);
            }

            return ent;
        }
        #endregion

        #region Variance
        public Double Variance(List<TExample> rows)
        {
            if (rows.Count == 0) return 0;
            List<Double> data = rows.Select(x => ToDouble(x.Result)).ToList();
            Double mean = data.Average();
            Double variance = data.Select(x => Math.Pow(x - mean, 2)).Average();
            return variance;
        }
        #endregion

        #region Static Methods
        private static Double ToDouble(TResult result)
        {
            return (Double)(object)result;
        }

        private static object ToObject(String val)
        {
            Double valDouble = 0.0;
            if (Double.TryParse(val, out valDouble)) return valDouble;
            else return val;
                
        }
        #endregion
    }
}
