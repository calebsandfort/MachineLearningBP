using Abp.AutoMapper;
using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Entities.Movies.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Movies
{
    [AutoMap(typeof(MovieStatLine))]
    public class MovieStatLineScrapeDto
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

        public String RatingString { get; set; }
        public String GenreString { get; set; }

        public String MicroSeriesString { get; set; }
        public String MacroSeriesString { get; set; }
        public String BrandString { get; set; }
    }
}
