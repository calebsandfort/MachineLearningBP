using System;
using System.Collections.Generic;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Movies
{
    [Table("Movies")]
    public class Movie : Sample, SampleMedium<MovieYear, MovieStatLine>
    {
        public String Title { get; set; }
        public String MojoId { get; set; }
        public DateTime WideRelease { get; set; }
        public bool Completed { get; set; }

        [ForeignKey("SampleId")]
        public virtual ICollection<MovieStatLine> StatLines { get; set; }

        [ForeignKey("TimeGroupingId")]
        public virtual MovieYear TimeGrouping { get; set; }
        public virtual int TimeGroupingId { get; set; }
    }
}
