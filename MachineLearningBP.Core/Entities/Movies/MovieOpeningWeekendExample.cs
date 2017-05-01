using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.Framework;

namespace MachineLearningBP.Entities.Movies
{
    [Table("MovieOpeningWeekendExamples")]
    public class MovieOpeningWeekendExample : ExampleMinimum<MovieStatLine, Double, MovieExampleGenerationInfo>
    {
        public DateTime WideRelease { get; set; }

        public override MovieStatLine StatLine { get; set; }
        public override int StatLineId { get; set; }

        public override bool PythonIndexOnly
        {
            get { return true; }
        }

        public override void SetData(MovieExampleGenerationInfo info)
        {
            this.WideRelease = info.Movie.WideRelease;

            List<Double> numericData = new List<double>();
            numericData.Add(info.StatLine.Runtime);
            numericData.Add(info.StatLine.TheaterCount);
            //numericData.Add(info.StatLine.Budget);
            this.NumericData = numericData;

            List<Double> ordinalData = new List<double>();
            ordinalData.Add((int)info.StatLine.MpaaRating);
            this.OrdinalData = ordinalData;

            List<String> nominalData = new List<String>();
            nominalData.Add(((int)info.StatLine.Genre).ToString());
            nominalData.Add(((int)info.StatLine.MacroSeries).ToString());
            nominalData.Add(((int)info.StatLine.MicroSeries).ToString());
            nominalData.Add(((int)info.StatLine.Brand).ToString());
            this.NominalData = nominalData;

            List<bool> asymmBinaryData = new List<bool>();
            asymmBinaryData.Add(info.StatLine.Brand.IsMegaBrand());
            asymmBinaryData.Add(info.StatLine.Brand.IsAnimationBrand());
            this.AsymmBinaryData = asymmBinaryData;
        }

        public override void SetResult(MovieExampleGenerationInfo info)
        {
            this.Result = info.StatLine.Opening;
        }
    }
}
