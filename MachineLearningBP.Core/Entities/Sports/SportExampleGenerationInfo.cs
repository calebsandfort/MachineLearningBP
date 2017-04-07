using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports
{
    public abstract class SportExampleGenerationInfo<TSample, TStatLine, TTimeGrouping, TParticipant> : ExampleGenerationInfo
        where TSample : Sample
        where TStatLine : StatLine
        where TTimeGrouping : TimeGrouping
        where TParticipant : Participant
    {
        #region Properties
        public int RollingWindowPeriod { get; set; }
        public Double ScaleFactor { get; set; }

        public abstract TSample Game { get; set; }
        public abstract List<TSample> Games { get; set; }
        public abstract List<TParticipant> Teams { get; set; }
        public bool Home { get; set; }

        public abstract TStatLine TeamStatLine1 { get; set; }
        public abstract TStatLine TeamStatLine2 { get; set; }
        public abstract TParticipant Team1 { get; set; }
        public abstract TParticipant Team2 { get; set; }
        public abstract List<TSample> Team1LastNGames { get; set; }
        public abstract List<TStatLine> Team1LastNStatLines { get; set; }
        public abstract List<TStatLine> Team1LastNOpponentStatLines { get; set; }
        public abstract List<TSample> Team2LastNGames { get; set; }
        public abstract List<TStatLine> Team2LastNStatLines { get; set; }
        public abstract List<TStatLine> Team2LastNOpponentStatLines { get; set; }
        #endregion

        #region Constructors
        protected SportExampleGenerationInfo(TSample game, List<TSample> games, List<TParticipant> teams, bool home, int rollingWindowPeriod, Double scaleFactor) { }
        

        //public SportExampleGenerationInfo(TSample game, List<TSample> games, List<TParticipant> teams, bool home, int rollingWindowPeriod, Double scaleFactor)
        //{
        //    //this.RollingWindowPeriod = rollingWindowPeriod;
        //    //this.ScaleFactor = scaleFactor;

        //    //this.Game = game;
        //    //this.Games = games;
        //    //this.Teams = teams;
        //    //this.Home = home;

        //    //this.TeamStatLine1 = game.StatLines.First(y => y.Home == home);
        //    //this.TeamStatLine2 = game.StatLines.First(y => y.Home != home);

        //    //this.Team1 = teams.Single(x => x.Id == this.TeamStatLine1.ParticipantId);
        //    //this.Team2 = teams.Single(x => x.Id == this.TeamStatLine2.ParticipantId);

        //    //this.Team1LastNGames = games.OrderByDescending(x => x.Date).Where(x => x.Date < game.Date && x.StatLines.Any(y => y.ParticipantId == this.Team1.Id)).Take(this.RollingWindowPeriod).ToList();
        //    //this.Team1LastNStatLines = this.Team1LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId == this.Team1.Id)).ToList();
        //    //this.Team1LastNOpponentStatLines = this.Team1LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId != this.Team1.Id)).ToList();

        //    //this.Team2LastNGames = games.OrderByDescending(x => x.Date).Where(x => x.Date < game.Date && x.StatLines.Any(y => y.ParticipantId == this.Team2.Id)).Take(this.RollingWindowPeriod).ToList();
        //    //this.Team2LastNStatLines = this.Team2LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId == this.Team2.Id)).ToList();
        //    //this.Team2LastNOpponentStatLines = this.Team2LastNGames.Select(x => x.StatLines.First(y => y.ParticipantId != this.Team2.Id)).ToList();
        //}
        #endregion
    }
}
