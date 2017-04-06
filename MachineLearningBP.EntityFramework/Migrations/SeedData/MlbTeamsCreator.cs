using MachineLearningBP.Entities.Sports.Mlb;
using MachineLearningBP.EntityFramework;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MachineLearningBP.Migrations.SeedData
{
    public class MlbTeamsCreator
    {
        private readonly MachineLearningBPDbContext _context;

        public MlbTeamsCreator(MachineLearningBPDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.MlbTeams.Count() == 0)
            {
                _context.MlbTeams.AddOrUpdate(
                    x => x.Name,
                    new MlbTeam { Name = "Atlanta" },
                    new MlbTeam { Name = "Pittsburgh" },
                    new MlbTeam { Name = "Boston" },
                    new MlbTeam { Name = "Detroit" },
                    new MlbTeam { Name = "Washington" },
                    new MlbTeam { Name = "Philadelphia" },
                    new MlbTeam { Name = "LA Dodgers" },
                    new MlbTeam { Name = "Colorado" },
                    new MlbTeam { Name = "San Francisco" },
                    new MlbTeam { Name = "San Diego" },
                    new MlbTeam { Name = "NY Yankees" },
                    new MlbTeam { Name = "Baltimore" },
                    new MlbTeam { Name = "Toronto" },
                    new MlbTeam { Name = "Tampa Bay" },
                    new MlbTeam { Name = "Miami" },
                    new MlbTeam { Name = "NY Mets" },
                    new MlbTeam { Name = "Oakland" },
                    new MlbTeam { Name = "Texas" },
                    new MlbTeam { Name = "Kansas City" },
                    new MlbTeam { Name = "Houston" },
                    new MlbTeam { Name = "Minnesota" },
                    new MlbTeam { Name = "Chi. White Sox" },
                    new MlbTeam { Name = "Chi. Cubs" },
                    new MlbTeam { Name = "Milwaukee" },
                    new MlbTeam { Name = "Cincinnati" },
                    new MlbTeam { Name = "St. Louis" },
                    new MlbTeam { Name = "Cleveland" },
                    new MlbTeam { Name = "Arizona" },
                    new MlbTeam { Name = "Seattle" },
                    new MlbTeam { Name = "LA Angels" }
                );
            }

            _context.SaveChanges();
        }
    }
}
