using MachineLearningBP.EntityFramework;
using MachineLearningBP.Seasons;
using MachineLearningBP.Teams;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Migrations.SeedData
{
    public class TeamsCreator
    {
        private readonly MachineLearningBPDbContext _context;

        public TeamsCreator(MachineLearningBPDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.NbaTeams.Count() == 0)
            {
                _context.NbaTeams.AddOrUpdate(
                    x => x.EspnName,
                    new NbaTeam { EspnName = "Atlanta Hawks", CoversName = "Atlanta", City = "Atlanta", Mascot = "Hawks", DraftKingsIdentifier = 135, DraftKingsAbbr = "ATL", EspnAbbr = "atl", BasketballReferenceAbbr = "ATL" },
                    new NbaTeam { EspnName = "Boston Celtics", CoversName = "Boston", City = "Boston", Mascot = "Celtics", DraftKingsIdentifier = 125, DraftKingsAbbr = "BOS", EspnAbbr = "bos", BasketballReferenceAbbr = "BOS" },
                    new NbaTeam { EspnName = "Brooklyn Nets", CoversName = "Brooklyn", City = "Brooklyn", Mascot = "Nets", DraftKingsIdentifier = 126, DraftKingsAbbr = "BKN", EspnAbbr = "bkn", BasketballReferenceAbbr = "BRK" },
                    new NbaTeam { EspnName = "Charlotte Hornets", CoversName = "Charlotte", City = "Charlotte", Mascot = "Hornets", DraftKingsIdentifier = 136, DraftKingsAbbr = "CHA", EspnAbbr = "cha", BasketballReferenceAbbr = "CHO" },
                    new NbaTeam { EspnName = "Chicago Bulls", CoversName = "Chicago", City = "Chicago", Mascot = "Bulls", DraftKingsIdentifier = 130, DraftKingsAbbr = "CHI", EspnAbbr = "chi", BasketballReferenceAbbr = "CHI" },
                    new NbaTeam { EspnName = "Cleveland Cavaliers", CoversName = "Cleveland", City = "Cleveland", Mascot = "Cavaliers", DraftKingsIdentifier = 131, DraftKingsAbbr = "CLE", EspnAbbr = "cle", BasketballReferenceAbbr = "CLE" },
                    new NbaTeam { EspnName = "Dallas Mavericks", CoversName = "Dallas", City = "Dallas", Mascot = "Mavericks", DraftKingsIdentifier = 140, DraftKingsAbbr = "DAL", EspnAbbr = "dal", BasketballReferenceAbbr = "DAL" },
                    new NbaTeam { EspnName = "Denver Nuggets", CoversName = "Denver", City = "Denver", Mascot = "Nuggets", DraftKingsIdentifier = 145, DraftKingsAbbr = "DEN", EspnAbbr = "den", BasketballReferenceAbbr = "DEN" },
                    new NbaTeam { EspnName = "Detroit Pistons", CoversName = "Detroit", City = "Detroit", Mascot = "Pistons", DraftKingsIdentifier = 132, DraftKingsAbbr = "DET", EspnAbbr = "det", BasketballReferenceAbbr = "DET" },
                    new NbaTeam { EspnName = "Golden State Warriors", CoversName = "Golden State", City = "Golden State", Mascot = "Warriors", DraftKingsIdentifier = 150, DraftKingsAbbr = "GSW", EspnAbbr = "gs", BasketballReferenceAbbr = "GSW" },
                    new NbaTeam { EspnName = "Houston Rockets", CoversName = "Houston", City = "Houston", Mascot = "Rockets", DraftKingsIdentifier = 141, DraftKingsAbbr = "HOU", EspnAbbr = "hou", BasketballReferenceAbbr = "HOU" },
                    new NbaTeam { EspnName = "Indiana Pacers", CoversName = "Indiana", City = "Indiana", Mascot = "Pacers", DraftKingsIdentifier = 133, DraftKingsAbbr = "IND", EspnAbbr = "ind", BasketballReferenceAbbr = "IND" },
                    new NbaTeam { EspnName = "Los Angeles Clippers", CoversName = "L.A. Clippers", City = "Los Angeles", Mascot = "Clippers", DraftKingsIdentifier = 151, DraftKingsAbbr = "LAC", EspnAbbr = "lac", BasketballReferenceAbbr = "LAC" },
                    new NbaTeam { EspnName = "Los Angeles Lakers", CoversName = "L.A. Lakers", City = "Los Angeles", Mascot = "Lakers", DraftKingsIdentifier = 152, DraftKingsAbbr = "LAL", EspnAbbr = "lal", BasketballReferenceAbbr = "LAL" },
                    new NbaTeam { EspnName = "Memphis Grizzlies", CoversName = "Memphis", City = "Memphis", Mascot = "Grizzlies", DraftKingsIdentifier = 142, DraftKingsAbbr = "MEM", EspnAbbr = "mem", BasketballReferenceAbbr = "MEM" },
                    new NbaTeam { EspnName = "Miami Heat", CoversName = "Miami", City = "Miami", Mascot = "Heat", DraftKingsIdentifier = 137, DraftKingsAbbr = "MIA", EspnAbbr = "mia", BasketballReferenceAbbr = "MIA" },
                    new NbaTeam { EspnName = "Milwaukee Bucks", CoversName = "Milwaukee", City = "Milwaukee", Mascot = "Bucks", DraftKingsIdentifier = 134, DraftKingsAbbr = "MIL", EspnAbbr = "mil", BasketballReferenceAbbr = "MIL" },
                    new NbaTeam { EspnName = "Minnesota Timberwolves", CoversName = "Minnesota", City = "Minnesota", Mascot = "Timberwolves", DraftKingsIdentifier = 146, DraftKingsAbbr = "MIN", EspnAbbr = "min", BasketballReferenceAbbr = "MIN" },
                    new NbaTeam { EspnName = "New Orleans Pelicans", CoversName = "New Orleans", City = "New Orleans", Mascot = "Pelicans", DraftKingsIdentifier = 143, DraftKingsAbbr = "NOP", EspnAbbr = "no", BasketballReferenceAbbr = "NOP" },
                    new NbaTeam { EspnName = "New York Knicks", CoversName = "New York", City = "New York", Mascot = "Knicks", DraftKingsIdentifier = 127, DraftKingsAbbr = "NYK", EspnAbbr = "ny", BasketballReferenceAbbr = "NYK" },
                    new NbaTeam { EspnName = "Oklahoma City Thunder", CoversName = "Oklahoma City", City = "Oklahoma City", Mascot = "Thunder", DraftKingsIdentifier = 148, DraftKingsAbbr = "OKC", EspnAbbr = "okc", BasketballReferenceAbbr = "OKC" },
                    new NbaTeam { EspnName = "Orlando Magic", CoversName = "Orlando", City = "Orlando", Mascot = "Magic", DraftKingsIdentifier = 138, DraftKingsAbbr = "ORL", EspnAbbr = "orl", BasketballReferenceAbbr = "ORL" },
                    new NbaTeam { EspnName = "Philadelphia 76ers", CoversName = "Philadelphia", City = "Philadelphia", Mascot = "76ers", DraftKingsIdentifier = 128, DraftKingsAbbr = "PHI", EspnAbbr = "phi", BasketballReferenceAbbr = "PHI" },
                    new NbaTeam { EspnName = "Phoenix Suns", CoversName = "Phoenix", City = "Phoenix", Mascot = "Suns", DraftKingsIdentifier = 153, DraftKingsAbbr = "PHO", EspnAbbr = "phx", BasketballReferenceAbbr = "PHO" },
                    new NbaTeam { EspnName = "Portland Trail Blazers", CoversName = "Portland", City = "Portland", Mascot = "Trail Blazers", DraftKingsIdentifier = 147, DraftKingsAbbr = "POR", EspnAbbr = "por", BasketballReferenceAbbr = "POR" },
                    new NbaTeam { EspnName = "Sacramento Kings", CoversName = "Sacramento", City = "Sacramento", Mascot = "Kings", DraftKingsIdentifier = 154, DraftKingsAbbr = "SAC", EspnAbbr = "sac", BasketballReferenceAbbr = "SAC" },
                    new NbaTeam { EspnName = "San Antonio Spurs", CoversName = "San Antonio", City = "San Antonio", Mascot = "Spurs", DraftKingsIdentifier = 144, DraftKingsAbbr = "SAS", EspnAbbr = "sa", BasketballReferenceAbbr = "SAS" },
                    new NbaTeam { EspnName = "Toronto Raptors", CoversName = "Toronto", City = "Toronto", Mascot = "Raptors", DraftKingsIdentifier = 129, DraftKingsAbbr = "TOR", EspnAbbr = "tor", BasketballReferenceAbbr = "TOR" },
                    new NbaTeam { EspnName = "Utah Jazz", CoversName = "Utah", City = "Utah", Mascot = "Jazz", DraftKingsIdentifier = 149, DraftKingsAbbr = "UTA", EspnAbbr = "utah", BasketballReferenceAbbr = "UTA" },
                    new NbaTeam { EspnName = "Washington Wizards", CoversName = "Washington", City = "Washington", Mascot = "Wizards", DraftKingsIdentifier = 139, DraftKingsAbbr = "WAS", EspnAbbr = "wsh", BasketballReferenceAbbr = "WAS" }
                );
            }

            _context.SaveChanges();
        }
    }
}
