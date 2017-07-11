using MachineLearningBP.Entities.Sports.Nfl;
using MachineLearningBP.EntityFramework;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MachineLearningBP.Migrations.SeedData
{
    public class NflTeamsCreator
    {
        private readonly MachineLearningBPDbContext _context;

        public NflTeamsCreator(MachineLearningBPDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.NflTeams.Count() == 0)
            {
                _context.NflTeams.AddOrUpdate(
                    x => x.Name,
                    new NflTeam { Name = "Carolina", PfrId = "car", SavId = "CAR" },
                    new NflTeam { Name = "Denver", PfrId = "den", SavId = "DEN" },
                    new NflTeam { Name = "Tampa Bay", PfrId = "tam", SavId = "TB" },
                    new NflTeam { Name = "Atlanta", PfrId = "atl", SavId = "ATL" },
                    new NflTeam { Name = "Buffalo", PfrId = "buf", SavId = "BUF" },
                    new NflTeam { Name = "Baltimore", PfrId = "rav", SavId = "BAL" },
                    new NflTeam { Name = "Chicago", PfrId = "chi", SavId = "CHI" },
                    new NflTeam { Name = "Houston", PfrId = "htx", SavId = "HOU" },
                    new NflTeam { Name = "Green Bay", PfrId = "gnb", SavId = "GB" },
                    new NflTeam { Name = "Jacksonville", PfrId = "jax", SavId = "JAX" },
                    new NflTeam { Name = "L.A. Chargers", PfrId = "sdg", SavId = "SD" },
                    new NflTeam { Name = "Kansas City", PfrId = "kan", SavId = "KC" },
                    new NflTeam { Name = "Oakland", PfrId = "rai", SavId = "OAK" },
                    new NflTeam { Name = "New Orleans", PfrId = "nor", SavId = "NO" },
                    new NflTeam { Name = "Cincinnati", PfrId = "cin", SavId = "CIN" },
                    new NflTeam { Name = "N.Y. Jets", PfrId = "nyj", SavId = "NYJ" },
                    new NflTeam { Name = "Cleveland", PfrId = "cle", SavId = "CLE" },
                    new NflTeam { Name = "Philadelphia", PfrId = "phi", SavId = "PHI" },
                    new NflTeam { Name = "Minnesota", PfrId = "min", SavId = "MIN" },
                    new NflTeam { Name = "Tennessee", PfrId = "oti", SavId = "TEN" },
                    new NflTeam { Name = "Miami", PfrId = "mia", SavId = "MIA" },
                    new NflTeam { Name = "Seattle", PfrId = "sea", SavId = "SEA" },
                    new NflTeam { Name = "N.Y. Giants", PfrId = "nyg", SavId = "NYG" },
                    new NflTeam { Name = "Dallas", PfrId = "dal", SavId = "DAL" },
                    new NflTeam { Name = "Detroit", PfrId = "det", SavId = "DET" },
                    new NflTeam { Name = "Indianapolis", PfrId = "clt", SavId = "IND" },
                    new NflTeam { Name = "New England", PfrId = "nwe", SavId = "NE" },
                    new NflTeam { Name = "Arizona", PfrId = "crd", SavId = "ARI" },
                    new NflTeam { Name = "Pittsburgh", PfrId = "pit", SavId = "PIT" },
                    new NflTeam { Name = "Washington", PfrId = "was", SavId = "WAS" },
                    new NflTeam { Name = "L.A. Rams ", PfrId = "ram", SavId = "LA" },
                    new NflTeam { Name = "San Francisco", PfrId = "sfo", SavId = "SF" }
                );
            }

            _context.SaveChanges();
        }
    }
}
