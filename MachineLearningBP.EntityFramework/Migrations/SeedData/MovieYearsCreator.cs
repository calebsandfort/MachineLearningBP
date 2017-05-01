using MachineLearningBP.Entities.Movies;
using MachineLearningBP.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MachineLearningBP.Migrations.SeedData
{
    public class MovieYearsCreator
    {
        private readonly MachineLearningBPDbContext _context;

        public MovieYearsCreator(MachineLearningBPDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.MovieYears.Count() == 0)
            {
                List<MovieYear> movieYears = new List<MovieYear>();

                for(int i = 1985; i <= 2017; i++)
                {
                    movieYears.Add(new MovieYear { Start = new DateTime(i, 1, 1), End = new DateTime(i, 12, 31) });
                }

                _context.MovieYears.AddOrUpdate( x => x.Start, movieYears.ToArray());
            }

            _context.SaveChanges();
        }
    }
}
