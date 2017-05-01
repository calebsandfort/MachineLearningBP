using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Movies
{
    [AutoMap(typeof(Movie))]
    public class MovieScrapeDto
    {
        public String Title { get; set; }
        public String MojoId { get; set; }
        public DateTime WideRelease { get; set; }
        public bool Completed { get; set; }

        public MovieStatLineScrapeDto Stats { get; set; }
    }
}
