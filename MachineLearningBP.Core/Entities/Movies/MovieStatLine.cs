using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Entities.Movies.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Movies
{
    [Table("MovieStatLines")]
    public class MovieStatLine : StatLine, StatLineMinimum<Movie>
    {
        public Double Runtime { get; set; }
        public Double Budget { get; set; }
        public Double TheaterCount { get; set; }
        public Double TheaterAverage { get; set; }
        public Double WeekendDuration { get; set; }
        public Double Opening { get; set; }

        public MpaaRatings MpaaRating { get; set; }
        public MovieGenres Genre { get; set; }
        public MovieSeries MicroSeries { get; set; }
        public MovieSeries MacroSeries { get; set; }
        public MovieBrands Brand { get; set; }

        [ForeignKey("SampleId")]
        public Movie Sample { get; set; }
        public int SampleId { get; set; }
    }
}
