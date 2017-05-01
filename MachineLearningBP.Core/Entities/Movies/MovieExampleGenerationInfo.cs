using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Movies
{
    public class MovieExampleGenerationInfo : ExampleGenerationInfo
    {
        #region Properties
        public Movie Movie { get; set; }
        public MovieStatLine StatLine { get; set; }
        #endregion

        #region Ctor
        public MovieExampleGenerationInfo(Movie movie)
        {
            this.Movie = movie;
            this.StatLine = movie.StatLines.First();
        }
        #endregion
    }
}
