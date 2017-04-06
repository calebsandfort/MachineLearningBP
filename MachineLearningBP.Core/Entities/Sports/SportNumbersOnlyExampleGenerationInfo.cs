using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports
{
    public class SportExampleGenerationInfo<TSample, TStatLine, TTimeGrouping, TParticipant> : ExampleGenerationInfo
        where TSample : Game<TStatLine, TTimeGrouping>
        where TStatLine : SportStatLine<TStatLine, TTimeGrouping>
        where TTimeGrouping : Season<TStatLine, TTimeGrouping>
        where TParticipant : Participant
    {
        #region Properties
        public int RollingWindowPeriod { get; set; }
        public Double ScaleFactor { get; set; }

        public TSample Game { get; set; }
        public List<TSample> Games { get; set; }
        public List<TParticipant> Teams { get; set; }
        public bool Home { get; set; }

        public TStatLine TeamStatLine1 { get; set; }
        public TStatLine TeamStatLine2 { get; set; }
        public TParticipant Team1 { get; set; }
        public TParticipant Team2 { get; set; }
        public List<TSample> Team1LastNGames { get; set; }
        public List<TStatLine> Team1LastNStatLines { get; set; }
        public List<TStatLine> Team1LastNOpponentStatLines { get; set; }
        public List<TSample> Team2LastNGames { get; set; }
        public List<TStatLine> Team2LastNStatLines { get; set; }
        public List<TStatLine> Team2LastNOpponentStatLines { get; set; }
        #endregion

        #region Constructors
        public SportExampleGenerationInfo(TSample game, List<TSample> games, List<TParticipant> teams, bool home, int rollingWindowPeriod, Double scaleFactor)
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
