using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports.Mlb
{
    public class MlbExampleGenerationInfo : SportExampleGenerationInfo<MlbGame, MlbStatLine, MlbSeason, MlbTeam>
    {
        #region Properties
        sealed public override MlbGame Game { get; set; }
        sealed public override List<MlbGame> Games { get; set; }
        sealed public override List<MlbTeam> Teams { get; set; }
        sealed public override MlbStatLine TeamStatLine1 { get; set; }
        sealed public override MlbStatLine TeamStatLine2 { get; set; }
        sealed public override MlbTeam Team1 { get; set; }
        sealed public override MlbTeam Team2 { get; set; }
        sealed public override List<MlbGame> Team1LastNGames { get; set; }
        sealed public override List<MlbStatLine> Team1LastNStatLines { get; set; }
        sealed public override List<MlbStatLine> Team1LastNOpponentStatLines { get; set; }
        sealed public override List<MlbGame> Team2LastNGames { get; set; }
        sealed public override List<MlbStatLine> Team2LastNStatLines { get; set; }
        sealed public override List<MlbStatLine> Team2LastNOpponentStatLines { get; set; } 
        #endregion
    
        #region Constructor
        public MlbExampleGenerationInfo(MlbGame game, List<MlbGame> games, List<MlbTeam> teams, bool home, int rollingWindowPeriod, double scaleFactor)
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

            this.Team1LastNGames = games.OrderByDescending(x => x.Date).Where(x => x.Date < game.Date && x.StatLines.Any(y => y.ParticipantId == this.Team1.Id)).Take(this.RollingWindowPeriod).ToList();
            this.Team1LastNStatLines = this.Team1LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId == this.Team1.Id)).ToList();
            this.Team1LastNOpponentStatLines = this.Team1LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId != this.Team1.Id)).ToList();

            this.Team2LastNGames = games.OrderByDescending(x => x.Date).Where(x => x.Date < game.Date && x.StatLines.Any(y => y.ParticipantId == this.Team2.Id)).Take(this.RollingWindowPeriod).ToList();
            this.Team2LastNStatLines = this.Team2LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId == this.Team2.Id)).ToList();
            this.Team2LastNOpponentStatLines = this.Team2LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId != this.Team2.Id)).ToList();
        }
        #endregion
    }
}
