using System;
using System.Collections.Generic;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Movies
{
    [Table("MovieYears")]
    public class MovieYear : TimeGrouping, TimeGroupingMinimum<Movie>
    {
        [ForeignKey("TimeGroupingId")]
        public ICollection<Movie> Samples { get; set; }
    }
}
