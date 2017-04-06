using MachineLearningBP.Entities.Sports.Mlb;
using MachineLearningBP.EntityFramework;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MachineLearningBP.Migrations.SeedData
{
    public class MlbSeasonsCreator
    {
        private readonly MachineLearningBPDbContext _context;

        public MlbSeasonsCreator(MachineLearningBPDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.MlbSeasons.Count() == 0)
            {
                _context.MlbSeasons.AddOrUpdate(
                    x => x.Start,
                    new MlbSeason { Start = new DateTime(2014, 3, 30), End = new DateTime(2014, 9, 30) },
                    new MlbSeason { Start = new DateTime(2015, 4, 5), End = new DateTime(2015, 10, 4) },
                    new MlbSeason { Start = new DateTime(2016, 4, 3), End = new DateTime(2016, 10, 2) },
                    new MlbSeason { Start = new DateTime(2017, 4, 2), End = new DateTime(2017, 10, 1) }
                );
            }

            _context.SaveChanges();
        }
    }
}
