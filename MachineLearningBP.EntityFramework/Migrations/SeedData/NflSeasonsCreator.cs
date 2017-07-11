using MachineLearningBP.Entities.Sports.Nfl;
using MachineLearningBP.EntityFramework;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MachineLearningBP.Migrations.SeedData
{
    public class NflSeasonsCreator
    {
        private readonly MachineLearningBPDbContext _context;

        public NflSeasonsCreator(MachineLearningBPDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.NflSeasons.Count() == 0)
            {
                _context.NflSeasons.AddOrUpdate(
                    x => x.Start,
                    new NflSeason { Start = new DateTime(2013, 9, 5), End = new DateTime(2013, 12, 29) },
                    new NflSeason { Start = new DateTime(2014, 9, 4), End = new DateTime(2014, 12, 28) },
                    new NflSeason { Start = new DateTime(2015, 9, 10), End = new DateTime(2016, 1, 3) },
                    new NflSeason { Start = new DateTime(2016, 9, 8), End = new DateTime(2017, 1, 1) },
                    new NflSeason { Start = new DateTime(2017, 9, 7), End = new DateTime(2017, 12, 31) }
                );
            }

            _context.SaveChanges();
        }
    }
}
