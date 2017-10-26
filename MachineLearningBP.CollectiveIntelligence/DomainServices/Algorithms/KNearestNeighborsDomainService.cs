using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Shared.GuerillaTimer;
using System.Threading;
using System.Collections.Concurrent;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using Abp.BackgroundJobs;
using MachineLearningBP.Shared.CommandRunner;
using System.IO;
using System.Globalization;
using MachineLearningBP.CollectiveIntelligence.Framework;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms
{
    public class KNearestNeighborsDomainService<TExample, TStatLine, TResult> : BaseDomainService, IKNearestNeighborsDomainService<TExample, TStatLine, TResult>
        where TExample : Example<TStatLine, TResult>, new()
        where TStatLine : StatLine
    {
        #region Properties
        readonly String rdataPathAndName = "C:\\Users\\csandfort\\Documents\\Visual Studio 2017\\Projects\\MachineLearningBP\\MachineLearningBP.CollectiveIntelligence\\DomainServices\\Algorithms\\Scripts\\R\\guerilladata.R";
        Object testAlgorithmLock = new Object();
        Object crossValidateLock = new Object();

        private readonly IOptimizationDomainService _optimizationDomainService;
        public readonly ICommandRunner _commandRunner;
        #endregion

        #region Constructor
        public KNearestNeighborsDomainService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy,
            ISettingManager settingManager, IOptimizationDomainService optimizationDomainService, IBackgroundJobManager backgroundJobManager, ICommandRunner commandRunner)
            : base(sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
        {
            _optimizationDomainService = optimizationDomainService;
            _commandRunner = commandRunner;
        }
        #endregion

        #region DivideData
        public void DivideData(TExample[] data, out TExample[] trainSet, out TExample[] testSet, double test = .05)
        {
            List<TExample> trainSetTemp = new List<TExample>();
            List<TExample> testSetTemp = new List<TExample>();

            Random random = new Random(DateTime.Now.Millisecond);

            foreach (TExample example in data)
            {
                if (random.NextDouble() < test)
                    testSetTemp.Add(example);
                else
                    trainSetTemp.Add(example);
            }

            trainSet = trainSetTemp.ToArray();
            testSet = testSetTemp.ToArray();
        }
        #endregion

        #region Euclidean
        public ValueIndexPair<double> Euclidean(TExample v1, TExample v2, int index)
        {
            Double d = 0;

            for (int i = 0; i < v1.OrdinalData.Count; i++)
            {
                d += Math.Pow(v1.OrdinalData[i] - v2.OrdinalData[i], 2.0);
            }

            return new ValueIndexPair<double>(Math.Sqrt(d), index);
        }
        #endregion

        #region GetDistances
        public ValueIndexPair<Double>[][] GetDistances(TExample[] data, TExample[] v1)
        {
            int v1Length = v1.Length;
            ValueIndexPair<Double>[][] distanceList = new ValueIndexPair<double>[v1Length][];

            try
            {
                int dataLength = data.Length;

                for (int j = 0; j < v1Length; j++)
                {
                    distanceList[j] = new ValueIndexPair<double>[dataLength];
                }

                for (int i = 0; i < dataLength; i++)
                {
                    for (int j = 0; j < v1Length; j++)
                    {
                        distanceList[j][i] = this.Euclidean(v1[j], data[i], i);
                    }
                }

                for (int j = 0; j < v1Length; j++)
                {
                    distanceList[j] = distanceList[j].OrderBy(x => x.Value).ToArray();
                }

                return distanceList;
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }
        #endregion

        #region algf
        #region KnnEstimate
        public double[][] KnnEstimate(TExample[] data, TExample[] v1, int[] ks)
        {
            ValueIndexPair<Double>[][] distanceList = this.GetDistances(data, v1);

            Double[][] avgs = new double[v1.Length][];
            for (int j = 0; j < v1.Length; j++)
            {
                avgs[j] = ks.Select(k => distanceList[j].Take(k).Average(x => ToDouble(data[x.Index].Result))).ToArray();
            }

            return avgs;
        }
        #endregion 

        #region WeightedKnn
        public double[][] WeightedKnn(TExample[] data, TExample[] v1, Func<Double, Double> weightf, int[] ks)
        {
            ValueIndexPair<Double>[][] distanceList = this.GetDistances(data, v1);
            Double avg = 0.0;
            Double totalWeight = 0.0;
            int k = 0;

            Double[][] avgs = new double[v1.Length][];
            for (int j = 0; j < v1.Length; j++)
            {
                avgs[j] = ks.Select(x => 0.0).ToArray();
                for (int i = 0; i < ks.Length; i++)
                {
                    k = ks[i];
                    avg = distanceList[j].Take(k).Sum(x => weightf(x.Value) * ToDouble(data[x.Index].Result));
                    totalWeight = distanceList[j].Take(k).Sum(x => weightf(x.Value));

                    if (totalWeight == 0)
                        avgs[j][i] = 0;
                    else
                        avgs[j][i] = avg / totalWeight;
                }
            }

            return avgs;
        }
        #endregion
        #endregion

        #region TestAlgorithm
        public Double[] TestAlgorithm(Func<TExample[], TExample[], int[], Double[][]> algf, TExample[] trainSet, TExample[] testSet, int[] ks)
        {
            Double[] errors = ks.Select(x => 0.0).ToArray();

            Double[][] guesses = algf(trainSet, testSet, ks);
            //Double[][] errors = new double[testSet.Length][];

            for (int j = 0; j < testSet.Length; j++)
            {
                for (int i = 0; i < ks.Length; i++)
                {
                    errors[i] += Math.Pow(ToDouble(testSet[i].Result) - guesses[j][i], 2.0);
                }
            }

            errors = errors.Select(x => x / testSet.Length).ToArray();

            return errors;
        }
        #endregion

        #region CrossValidate
        public List<KNearestNeighborsCrossValidateResult> CrossValidate(KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> input, bool doTimer = true)
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy, prefix: $"CrossValidate: {input}", show: doTimer))
            {
                Func<TExample[], TExample[], int[], Double[][]> algf;
                Func<Double, Double> weightf;

                switch (input.GuessMethod)
                {
                    case KNearestNeighborsGuessMethods.KnnEstimate:
                        algf = (d, t, k) => KnnEstimate(d, t, k);
                        break;
                    case KNearestNeighborsGuessMethods.WeightedKnn:
                        switch (input.WeightMethod)
                        {
                            case KNearestNeighborsWeightMethods.InverseWeight:
                                weightf = (d) => InverseWeight(d, input.InverseWeightNum, input.InverseWeightConstant);
                                break;
                            case KNearestNeighborsWeightMethods.SubtractWeight:
                                weightf = (d) => SubtractWeight(d, input.SubtractWeightConstant);
                                break;
                            case KNearestNeighborsWeightMethods.Gaussian:
                            default:
                                weightf = (d) => Gaussian(d, input.GaussianSigma);
                                break;
                        }

                        algf = (d, t, k) => WeightedKnn(d, t, weightf, k);
                        break;
                    default:
                        algf = (d, t, k) => KnnEstimate(d, t, k);
                        break;
                }

                Double[] crossValidateResults = this.CrossValidate(algf, input.Data, input.Ks, trials: input.Trials);

                List<KNearestNeighborsCrossValidateResult> results = new List<KNearestNeighborsCrossValidateResult>();

                for(int i = 0; i < input.Ks.Length; i++)
                {
                    results.Add(new KNearestNeighborsCrossValidateResult(
                    input.GuessMethod,
                    input.WeightMethod,
                    input.Trials,
                    input.Ks[i],
                    crossValidateResults[i]));
                }

                return results;
            }
        }

        public Double[] CrossValidate(Func<TExample[], TExample[], int[], Double[][]> algf, TExample[] data, int[] ks, int trials = 100, Double test = .05)
        {
            Double[] errors = ks.Select(x => 0.0).ToArray();

            Parallel.For(0, trials,
                j =>
                {
                    try
                    {
                        TExample[] trainSet, testSet;
                        this.DivideData(data, out trainSet, out testSet, test);

                        Double[] errorsTemp = this.TestAlgorithm(algf, trainSet, testSet, ks);

                        lock (crossValidateLock)
                        {
                            for (int i = 0; i < errorsTemp.Length; i++)
                            {
                                errors[i] += errorsTemp[i];
                            }
                        }

                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                        throw ex;
                    }
                });

            //Double error = 0.0;
            //List<TExample> trainSet, testSet;

            //for (int i = 0; i < trials; i++)
            //{
            //    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"CrossValidate trial {i + 1} of {trials}..."));

            //    this.DivideData(data, out trainSet, out testSet, test);
            //    error += this.TestAlgorithm(algF, trainSet, testSet);
            //}

            errors = errors.Select(x => x / trials).ToArray();

            return errors;
        }
        #endregion

        #region FindOptimalParametersPython
        public List<KNearestNeighborsCrossValidateResult> FindOptimalParametersPython(KNearestNeighborsOptimalParametersInput<TExample, TStatLine, TResult> optimalParametersInput)
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                WritePythonDataFile(optimalParametersInput.Data);

                List<KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>> inputs = GetInputs(optimalParametersInput);
                List<KNearestNeighborsCrossValidateResult> results = new List<KNearestNeighborsCrossValidateResult>();

                foreach (KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> input in inputs)
                {
                    results.AddRange(input.GetPythonResults(this._commandRunner));
                }

                return results.OrderBy(x => x.Result).ToList();
            }
        }
        #endregion

        #region FindOptimalParametersPythonAndR
        public List<KNearestNeighborsCrossValidateResult> FindOptimalParametersPythonAndR(KNearestNeighborsOptimalParametersInput<TExample, TStatLine, TResult> optimalParametersInput)
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                Double[] daisyDistances = GetDaisyDistances(optimalParametersInput);
                WritePythonDataFile(optimalParametersInput.Data, daisyDistances, optimalParametersInput.ResultScale);

                List<KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>> inputs = GetInputs(optimalParametersInput);
                List<KNearestNeighborsCrossValidateResult> results = new List<KNearestNeighborsCrossValidateResult>();

                foreach (KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> input in inputs)
                {
                    results.AddRange(input.GetPythonResults(this._commandRunner));
                }

                return results.OrderBy(x => x.Result).ToList();
            }
        }
        #endregion

        #region FindOptimalParameters
        public List<KNearestNeighborsCrossValidateResult> FindOptimalParameters(KNearestNeighborsOptimalParametersInput<TExample, TStatLine, TResult> optimalParametersInput)
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                List<KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>> inputs = GetInputs(optimalParametersInput);

                List<KNearestNeighborsCrossValidateResult> results = new List<KNearestNeighborsCrossValidateResult>();
                foreach (KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> input in inputs)
                {
                    results.AddRange(this.CrossValidate(input));
                }

                return results.OrderBy(x => x.Result).ToList();
            }
        }
        #endregion

        #region GetInputs
        static List<KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>> GetInputs(KNearestNeighborsOptimalParametersInput<TExample, TStatLine, TResult> optimalParametersInput)
        {
            List<KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>> inputs = new List<KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>>();

            foreach (KNearestNeighborsGuessMethods guessMethod in optimalParametersInput.GuessMethods)
            {
                if (guessMethod == KNearestNeighborsGuessMethods.WeightedKnn)
                {
                    foreach (KNearestNeighborsWeightMethods weightMethod in optimalParametersInput.WeightMethods)
                    {
                        KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> input = new KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>();
                        input.GuessMethod = guessMethod;
                        input.WeightMethod = weightMethod;
                        input.Trials = optimalParametersInput.Trials;
                        input.Ks = optimalParametersInput.Ks;
                        input.SubtractWeightConstant = optimalParametersInput.SubtractWeightConstant;
                        input.DistanceMethod = optimalParametersInput.DistanceMethod;
                        input.Data = optimalParametersInput.Data;
                        inputs.Add(input);
                    }
                }
                else
                {
                    KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> input = new KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>();
                    input.GuessMethod = guessMethod;
                    input.Trials = optimalParametersInput.Trials;
                    input.Ks = optimalParametersInput.Ks;
                    input.SubtractWeightConstant = optimalParametersInput.SubtractWeightConstant;
                    input.DistanceMethod = optimalParametersInput.DistanceMethod;
                    input.Data = optimalParametersInput.Data;
                    inputs.Add(input);
                }
            }

            return inputs;
        } 
        #endregion

        #region AnnealingOptimize
        public OptimizeResult AnnealingOptimize(AnnealingOptimizeInput input, TExample[] data)
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                WritePythonDataFile(data);

                KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> crossValidateInput = new KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>();
                crossValidateInput.GuessMethod = input.GuessMethod;
                crossValidateInput.WeightMethod = input.WeightMethod;
                crossValidateInput.Trials = input.Trials;
                crossValidateInput.Ks = new int[] { input.K };
                crossValidateInput.SubtractWeightConstant = 30.0;
                crossValidateInput.Data = data;

                input.domain = data.First().OrdinalData.Select(x => new OptimizationRange { Lower = 0, Upper = 20 }).ToList();
                input.costf = CreateCostFunction(crossValidateInput);

                OptimizeResult result = this._optimizationDomainService.AnnealingOptimize(input);

                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Results: {String.Join(",", result.Vec)}"));

                return result;
            }
        }
        #endregion

        #region GeneticOptimize
        public OptimizeResult GeneticOptimize(GeneticOptimizeInput input, TExample[] data)
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> crossValidateInput = new KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>();
                crossValidateInput.GuessMethod = input.GuessMethod;
                crossValidateInput.WeightMethod = input.WeightMethod;
                crossValidateInput.Trials = input.Trials;
                crossValidateInput.Ks = new int[] { input.K };
                crossValidateInput.SubtractWeightConstant = 30.0;
                crossValidateInput.Data = data;

                input.domain = data.First().NumericData.Select(x => new OptimizationRange { Lower = 0, Upper = 10 }).ToList();
                input.costf = CreateCostFunction(crossValidateInput);

                OptimizeResult result = this._optimizationDomainService.GeneticOptimize(input);

                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Results: {String.Join(",", result.Vec)}"));

                return result;
            }
        }
        #endregion

        #region Rescale
        public TExample[] Rescale(TExample[] data, List<int> scale)
        {
            TExample[] rescaledData = new TExample[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                TExample example = data[i];
                TExample scaled = new TExample();
                scaled.NumericData = data[i].NumericData.Select((x, idx) => x * scale[idx]).ToList();
                scaled.Result = example.Result;
                rescaledData[i] = scaled;
            }

            return rescaledData;
        }
        #endregion

        #region CreateCostFunction
        public Func<List<int>, int, double> CreateCostFunction(KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult> input)
        {
            Func<List<int>, int, double> costf = (s, i) =>
            {
                KNearestNeighborsOptimalParametersInput<TExample, TStatLine, TResult> optimalParametersInput = new KNearestNeighborsOptimalParametersInput<TExample, TStatLine, TResult>();
                optimalParametersInput.Data = Rescale(input.Data, s);

                Double[] daisyDistances = GetDaisyDistances(optimalParametersInput);
                WritePythonDataFile(optimalParametersInput.Data, daisyDistances, 1);

                input.Scale = s;
                List<KNearestNeighborsCrossValidateResult> results = input.GetPythonResults(this._commandRunner, $"_{i}");
                return results.First().Result;
            };

            return costf;
        } 
        #endregion

        #region Weight Functions
        #region InverseWeight
        public Double InverseWeight(Double dist, Double num = 1.0, Double constant = .1)
        {
            return num / (dist + constant);
        }
        #endregion

        #region SubtractWeight
        public Double SubtractWeight(Double dist, Double constant = 1.0)
        {
            if (dist > constant)
                return 0;
            else
                return constant - dist;
        }
        #endregion

        #region Gaussian
        public Double Gaussian(Double dist, Double sigma = 5.0)
        {
            return Math.Pow(Math.E, (Math.Pow(-dist, 2.0) / (2.0 * Math.Pow(sigma, 2.0))));
        }
        #endregion
        #endregion

        #region WritePythonDataFile
        public void WritePythonDataFile(TExample[] data, Double[] gowerDistances = null, Double resultScale = 1)
        {
            using (StreamWriter guerillaknnPyFile = new StreamWriter("C:\\Users\\csandfort\\Documents\\Visual Studio 2017\\Projects\\MachineLearningBP\\MachineLearningBP.CollectiveIntelligence\\DomainServices\\Algorithms\\Scripts\\Python\\guerilladata.py", false))
            {
                guerillaknnPyFile.WriteLine("def getdata():");
                guerillaknnPyFile.WriteLine("    rows = []");

                int idx = 0;
                foreach (TExample example in data)
                {
                    guerillaknnPyFile.WriteLine($"    rows.append({{'index': {idx}, 'input': ({String.Join(", ", example.NumericData)}), 'result': {ToDouble(example.Result) * resultScale}}})");
                    //if(example.PythonIndexOnly) guerillaknnPyFile.WriteLine($"    rows.append({{'index': {idx}, 'result': {ToDouble(example.Result) * resultScale}}})");
                    //else guerillaknnPyFile.WriteLine($"    rows.append({{'index': {idx}, 'input': ({String.Join(", ", example.NumericData)}), 'result': {ToDouble(example.Result) * resultScale}}})");

                    idx++;
                }

                guerillaknnPyFile.WriteLine("    return rows");

                if(gowerDistances != null && gowerDistances.Count() > 0)
                {
                    guerillaknnPyFile.WriteLine();

                    guerillaknnPyFile.WriteLine($"n = {data.Count()}");
                    guerillaknnPyFile.WriteLine($"daisydistances = [{String.Join(",", gowerDistances)}]");

                    guerillaknnPyFile.WriteLine();

                    guerillaknnPyFile.WriteLine("def getdaisydistance(row1, row2):");
                    guerillaknnPyFile.WriteLine("    i = row1['index'] + 1");
                    guerillaknnPyFile.WriteLine("    j = row2['index'] + 1");
                    guerillaknnPyFile.WriteLine();
                    guerillaknnPyFile.WriteLine("    if i > j:");
                    guerillaknnPyFile.WriteLine("        t = i");
                    guerillaknnPyFile.WriteLine("        i = j");
                    guerillaknnPyFile.WriteLine("        j = t");
                    guerillaknnPyFile.WriteLine();
                    guerillaknnPyFile.WriteLine("    idx = int((n*(i-1) - i*((i-1)/2) + (j-i)) - 1)");
                    guerillaknnPyFile.WriteLine();
                    guerillaknnPyFile.WriteLine("    return daisydistances[idx]");
                }

                guerillaknnPyFile.Close();
            }
        }
        #endregion

        #region GetDaisyDistances
        public Double[] GetDaisyDistances(KNearestNeighborsOptimalParametersInput<TExample, TStatLine, TResult> optimalParametersInput)
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                WriteRDataFile(optimalParametersInput);
                String strResult = _commandRunner.RunCmd("RScript", rdataPathAndName);
                Double[] distances = strResult.Trim().Split(",".ToCharArray()).Select(x => Double.Parse(x)).ToArray();
                return distances;
            }
        } 
        #endregion

        #region WriteRDataFile
        public void WriteRDataFile(KNearestNeighborsOptimalParametersInput<TExample, TStatLine, TResult> optimalParametersInput)
        {
            try
            {
                using (StreamWriter guerillakdataRFile = new StreamWriter(rdataPathAndName, false))
                {
                    guerillakdataRFile.WriteLine("library(cluster)");
                    guerillakdataRFile.WriteLine();

                    guerillakdataRFile.WriteLine("getData <- function() {");

                    TExample dummy = optimalParametersInput.Data[0];
                    List<String> columns = new List<string>();

                    NumberFormatInfo noComma = new CultureInfo("en-US", false).NumberFormat;
                    noComma.NumberGroupSeparator = String.Empty;

                    //Numeric
                    double[] numericNaValues = { -1 };
                    for (int i = 0; i < dummy.NumericData.Count; i++)
                    {
                        guerillakdataRFile.WriteLine($" num{i} = c({String.Join(",", optimalParametersInput.Data.Select(x => x.NumericData[i].ToRDouble(noComma, numericNaValues)))})");
                        columns.Add($"num{i}");
                    }

                    //Ordinal
                    double[] ordinalNaValues = { -1 };
                    for (int i = 0; i < dummy.OrdinalData.Count; i++)
                    {
                        guerillakdataRFile.WriteLine($" ord{i} = ordered(c({String.Join(",", optimalParametersInput.Data.Select(x => x.OrdinalData[i].ToRInt(noComma, ordinalNaValues)))}))");
                        columns.Add($"ord{i}");
                    }

                    //Nominal
                    string[] nominalNaValues = { "" };
                    for (int i = 0; i < dummy.NominalData.Count; i++)
                    {
                        guerillakdataRFile.WriteLine($" nom{i} = factor(c({String.Join(",", optimalParametersInput.Data.Select(x => x.NominalData[i].ToRString(nominalNaValues)))}))");
                        columns.Add($"nom{i}");
                    }

                    //Asymm
                    for (int i = 0; i < dummy.AsymmBinaryData.Count; i++)
                    {
                        guerillakdataRFile.WriteLine($" asymm{i} = factor(c({String.Join(",", optimalParametersInput.Data.Select(x => x.AsymmBinaryData[i].ToRBool()))}))");
                        columns.Add($"asymm{i}");
                    }

                    //Symm
                    for (int i = 0; i < dummy.SymmBinaryData.Count; i++)
                    {
                        guerillakdataRFile.WriteLine($" symm{i} = factor(c({String.Join(",", optimalParametersInput.Data.Select(x => x.SymmBinaryData[i].ToRBool()))}))");
                        columns.Add($"symm{i}");
                    }

                    guerillakdataRFile.WriteLine($" return (data.frame({String.Join(",", columns)}))");

                    guerillakdataRFile.WriteLine("}");
                    guerillakdataRFile.WriteLine();
                    guerillakdataRFile.WriteLine("rows = getData()");
                    guerillakdataRFile.WriteLine();
                    guerillakdataRFile.Write($"D = daisy(rows, metric=\"{optimalParametersInput.DistanceMethod.ToString().ToLower()}\"");

                    if(dummy.AsymmBinaryData.Count > 0 || dummy.SymmBinaryData.Count > 0)
                    {
                        List<String> typeList = new List<string>();
                        if (dummy.AsymmBinaryData.Count > 0)
                        {
                            typeList.Add(String.Format("asymm = c({0})", String.Join(",", Enumerable.Range(0, dummy.AsymmBinaryData.Count).Select(x => $"\"asymm{x}\""))));
                        }

                        if (dummy.SymmBinaryData.Count > 0)
                        {
                            typeList.Add(String.Format("symm = c({0})", String.Join(",", Enumerable.Range(0, dummy.SymmBinaryData.Count).Select(x => $"\"symm{x}\""))));
                        }

                        guerillakdataRFile.Write($", type = list({String.Join(",", typeList)})");
                    }

                    guerillakdataRFile.Write(")");
                    guerillakdataRFile.WriteLine();
                    guerillakdataRFile.WriteLine();
                    guerillakdataRFile.WriteLine("cat(D,sep=\",\")");
                    guerillakdataRFile.Close();
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }
        #endregion

        #region Static Methods
        private static Double ToDouble(TResult result)
        {
            return (Double)(object)result;
        }

        public static double Add(ref double location1, double value)
        {
            double newCurrentValue = location1; // non-volatile read, so may be stale
            while (true)
            {
                double currentValue = newCurrentValue;
                double newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (newCurrentValue == currentValue)
                    return newValue;
            }
        }
        #endregion
    }
}
