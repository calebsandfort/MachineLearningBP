using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports.Nba
{
    public class NbaExampleGenerationInfo : SportExampleGenerationInfo<NbaGame, NbaStatLine, NbaSeason, NbaTeam>
    {
        #region Properties
        sealed public override NbaGame Game { get; set; }
        sealed public override List<NbaGame> Games { get; set; }
        sealed public override List<NbaTeam> Teams { get; set; }
        sealed public override NbaStatLine TeamStatLine1 { get; set; }
        sealed public override NbaStatLine TeamStatLine2 { get; set; }
        sealed public override NbaTeam Team1 { get; set; }
        sealed public override NbaTeam Team2 { get; set; }
        sealed public override List<NbaGame> Team1LastNGames { get; set; }
        sealed public override List<NbaStatLine> Team1LastNStatLines { get; set; }
        sealed public override List<NbaStatLine> Team1LastNOpponentStatLines { get; set; }
        sealed public override List<NbaGame> Team2LastNGames { get; set; }
        sealed public override List<NbaStatLine> Team2LastNStatLines { get; set; }
        sealed public override List<NbaStatLine> Team2LastNOpponentStatLines { get; set; } 
        #endregion
    
        #region Constructor
        public NbaExampleGenerationInfo(NbaGame game, List<NbaGame> games, List<NbaTeam> teams, bool home, int rollingWindowPeriod, double scaleFactor)
            : base(game, games, teams, home, rollingWindowPeriod, scaleFactor)
        {
            this.RollingWindowPeriod = rollingWindowPeriod;
            this.ScaleFactor = scaleFactor;

            this.Game = game;
            this.Games = games;
            this.Teams = teams;
            this.Home = home;

            this.TeamStatLine1 = game.StatLines.First(y => y.Home == home);
            this.TeamStatLine2 = game.StatLines.First(y => y.Home != home);

            this.Team1 = teams.Single(x => x.Id == this.TeamStatLine1.ParticipantId);
            this.Team2 = teams.Single(x => x.Id == this.TeamStatLine2.ParticipantId);

            this.Team1LastNGames = this.Games.OrderByDescending(x => x.Date).Where(x => x.Date < game.Date && x.StatLines.Any(y => y.ParticipantId == this.Team1.Id)).Take(this.RollingWindowPeriod).ToList();
            this.Team1LastNStatLines = this.Team1LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId == this.Team1.Id)).ToList();
            this.Team1LastNOpponentStatLines = this.Team1LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId != this.Team1.Id)).ToList();

            this.Team2LastNGames = this.Games.OrderByDescending(x => x.Date).Where(x => x.Date < game.Date && x.StatLines.Any(y => y.ParticipantId == this.Team2.Id)).Take(this.RollingWindowPeriod).ToList();
            this.Team2LastNStatLines = this.Team2LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId == this.Team2.Id)).ToList();
            this.Team2LastNOpponentStatLines = this.Team2LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId != this.Team2.Id)).ToList();
        }
        #endregion
    }
}
