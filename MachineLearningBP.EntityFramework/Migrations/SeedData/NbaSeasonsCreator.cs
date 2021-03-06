﻿using MachineLearningBP.Entities.Sports.Nba;
using MachineLearningBP.EntityFramework;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MachineLearningBP.Migrations.SeedData
{
    public class NbaSeasonsCreator
    {
        private readonly MachineLearningBPDbContext _context;

        public NbaSeasonsCreator(MachineLearningBPDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.NbaSeasons.Count() == 0)
            {
                _context.NbaSeasons.AddOrUpdate(
                    x => x.Start,
                    new NbaSeason { Start = new DateTime(2012, 10, 30), End = new DateTime(2013, 4, 17) },
                    new NbaSeason { Start = new DateTime(2013, 10, 29), End = new DateTime(2014, 4, 16) },
                    new NbaSeason { Start = new DateTime(2014, 10, 28), End = new DateTime(2015, 4, 15) },
                    new NbaSeason { Start = new DateTime(2015, 10, 27), End = new DateTime(2016, 4, 13) },
                    new NbaSeason { Start = new DateTime(2016, 10, 25), End = new DateTime(2017, 4, 12) }
                );
            }

            _context.SaveChanges();
        }
    }
}
