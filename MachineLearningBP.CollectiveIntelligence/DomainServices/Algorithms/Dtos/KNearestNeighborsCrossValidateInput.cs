using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Shared.CommandRunner;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class KNearestNeighborsCrossValidateInput<TExample, TStatLine, TResult>
        where TExample : Example<TStatLine, TResult>
        where TStatLine : StatLine
    {
        public string PythonFilePath = "C:\\Users\\csandfort\\Documents\\Visual Studio 2017\\Projects\\MachineLearningBP\\MachineLearningBP.CollectiveIntelligence\\DomainServices\\Algorithms\\Scripts\\Python\\";
        public TExample[] Data { get; set; }
        public KNearestNeighborsGuessMethods GuessMethod { get; set; }
        public KNearestNeighborsWeightMethods WeightMethod { get; set; }
        public int Trials { get; set; }
        public int[] Ks { get; set; }
        public List<int> Scale { get; set; }
        public TExample Observation { get; set; }

        //InverseWeight
        public double InverseWeightNum { get; set; }
        public double InverseWeightConstant { get; set; }

        //SubtractWeight
        public double SubtractWeightConstant { get; set; }

        //Gaussian
        public double GaussianSigma { get; set; }

        public KNearestNeighborsCrossValidateInput()
        {
            this.Trials = 100;
            this.Ks = new int[] { 5 };
            this.Scale = new List<int>();

            this.WeightMethod = KNearestNeighborsWeightMethods.None;
            this.InverseWeightNum = 1.0;
            this.InverseWeightConstant = .1;
            this.SubtractWeightConstant = 1.0;
            this.GaussianSigma = 5.0;
        }

        public override string ToString()
        {
            return ($"{this.GuessMethod}{(this.GuessMethod == KNearestNeighborsGuessMethods.WeightedKnn ? ", " + this.WeightMethod : string.Empty)}");
        }

        #region PythonWeightf
        public string PythonWeightf
        {
            get
            {
                string weightf = string.Empty;

                switch (this.WeightMethod)
                {
                    case KNearestNeighborsWeightMethods.InverseWeight:
                        weightf = $"return numpredict.inverseweight(dist, num={this.InverseWeightNum}, const={this.InverseWeightConstant})";
                        break;
                    case KNearestNeighborsWeightMethods.SubtractWeight:
                        weightf = $"return numpredict.inverseweight(dist, const={this.SubtractWeightConstant})";
                        break;
                    case KNearestNeighborsWeightMethods.Gaussian:
                    default:
                        weightf = $"return numpredict.gaussian(dist, sigma={this.GaussianSigma})";
                        break;
                }

                return weightf;
            }
        }
        #endregion

        #region PythonGuessf
        public string PythonGuessf
        {

            get
            {
                string guessf = string.Empty;

                switch (this.GuessMethod)
                {
                    case KNearestNeighborsGuessMethods.WeightedKnn:
                        guessf = $"return numpredict.weightedknn(d, v, ks, weightf=myweight)";
                        break;
                    case KNearestNeighborsGuessMethods.KnnEstimate:
                    default:
                        guessf = $"return numpredict.knnestimate(d, v, ks)";
                        break;
                }

                return guessf;
            }
        }
        #endregion

        #region PythonFileName
        public string PythonFileName(string suffix)
        {
            return $"{this.PythonFilePath}guerillaknn_{this.GuessMethod.ToString().ToLower()}_{this.WeightMethod.ToString().ToLower()}{suffix}.py";
        }
        #endregion

        #region WritePythonFile
        private void WritePythonFile(string suffix = "")
        {
            using (StreamWriter guerillaknnPyFile = new StreamWriter(this.PythonFileName(suffix), false))
            {
                guerillaknnPyFile.WriteLine("import numpredict");
                guerillaknnPyFile.WriteLine("import guerilladata");
                guerillaknnPyFile.WriteLine();
                guerillaknnPyFile.WriteLine("data = guerilladata.getdata()");
                if(this.Scale.Count > 0) guerillaknnPyFile.WriteLine($"data = numpredict.rescale(data, [{string.Join(",", this.Scale)}])");
                guerillaknnPyFile.WriteLine($"def myweight(dist): {this.PythonWeightf}");
                guerillaknnPyFile.WriteLine($"def myknn(d, v, ks): {this.PythonGuessf}");
                guerillaknnPyFile.WriteLine();
                if (this.Observation != null)
                {
                    guerillaknnPyFile.WriteLine($"result = myknn(data, [{string.Join(",", this.Observation.NumericalData)}], [{string.Join(",", this.Ks)}])");
                }
                else
                {
                    guerillaknnPyFile.WriteLine($"result = numpredict.crossvalidate(myknn, data, ks=[{string.Join(",", this.Ks)}], trials=25, test=0.05)");
                }
                guerillaknnPyFile.WriteLine("print(\",\".join([str(item) for item in result]))");
                guerillaknnPyFile.Close();
            }
        }
        #endregion

        #region GetPythonResults
        public List<KNearestNeighborsCrossValidateResult> GetPythonResults(ICommandRunner _commandRunner, string suffix = "")
        {
            this.WritePythonFile(suffix);

            string result = _commandRunner.RunCmd("python", this.PythonFileName(suffix)).Trim();

            return result.Split(",".ToCharArray()).Select(x => double.Parse(x)).Select((x, idx) => new KNearestNeighborsCrossValidateResult(
                    this.GuessMethod,
                    this.WeightMethod,
                    this.Trials,
                    this.Ks[idx],
                    x)).ToList();
        }
        #endregion
    }

    public enum KNearestNeighborsGuessMethods
    {
        KnnEstimate,
        WeightedKnn
    }

    public enum KNearestNeighborsWeightMethods
    {
        None,
        InverseWeight,
        SubtractWeight,
        Gaussian
    }
}
