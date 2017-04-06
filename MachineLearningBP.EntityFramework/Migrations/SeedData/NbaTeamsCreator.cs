using MachineLearningBP.Entities.Sports.Nba;
using MachineLearningBP.EntityFramework;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MachineLearningBP.Migrations.SeedData
{
    public class NbaTeamsCreator
    {
        private readonly MachineLearningBPDbContext _context;

        public NbaTeamsCreator(MachineLearningBPDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.NbaTeams.Count() == 0)
            {
                _context.NbaTeams.AddOrUpdate(
                    x => x.Name,
                    new NbaTeam { Name = "Atlanta" },
                    new NbaTeam { Name = "Boston" },
                    new NbaTeam { Name = "Brooklyn" },
                    new NbaTeam { Name = "Charlotte" },
                    new NbaTeam { Name = "Chicago" },
                    new NbaTeam { Name = "Cleveland" },
                    new NbaTeam { Name = "Dallas" },
                    new NbaTeam { Name = "Denver" },
                    new NbaTeam { Name = "Detroit" },
                    new NbaTeam { Name = "Golden State" },
                    new NbaTeam { Name = "Houston" },
                    new NbaTeam { Name = "Indiana" },
                    new NbaTeam { Name = "L.A. Clippers" },
                    new NbaTeam { Name = "L.A. Lakers" },
                    new NbaTeam { Name = "Memphis" },
                    new NbaTeam { Name = "Miami" },
                    new NbaTeam { Name = "Milwaukee" },
                    new NbaTeam { Name = "Minnesota" },
                    new NbaTeam { Name = "New Orleans" },
                    new NbaTeam { Name = "New York" },
                    new NbaTeam { Name = "Oklahoma City" },
                    new NbaTeam { Name = "Orlando" },
                    new NbaTeam { Name = "Philadelphia" },
                    new NbaTeam { Name = "Phoenix" },
                    new NbaTeam { Name = "Portland" },
                    new NbaTeam { Name = "Sacramento" },
                    new NbaTeam { Name = "San Antonio" },
                    new NbaTeam { Name = "Toronto" },
                    new NbaTeam { Name = "Utah" },
                    new NbaTeam { Name = "Washington" }
                );
            }

            _context.SaveChanges();
        }
    }
}
