using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MachineLearningBP.Entities.Sports.Nfl
{
    [Table("NflGames")]
    public class NflGame : Sample, Game<NflSeason, NflStatLine>
    {
        [ForeignKey("SampleId")]
        public virtual ICollection<NflStatLine> StatLines { get; set; }

        [ForeignKey("TimeGroupingId")]
        public virtual NflSeason TimeGrouping { get; set; }
        public virtual int TimeGroupingId { get; set; }

        public DateTime Date { get; set; }
        public int EspnIdentifier { get; set; }
        public bool Completed { get; set; }
        public double Total { get; set; }

        public Double Spread { get; set; }

        #region Computed
        [NotMapped]
        NflStatLine awayStatLine = null;
        [NotMapped]
        public NflStatLine AwayStatLine
        {
            get
            {
                if (awayStatLine == null)
                {
                    awayStatLine = this.StatLines.First(x => !x.Home);
                }

                return awayStatLine;
            }
        }

        [NotMapped]
        NflStatLine homeStatLine = null;
        [NotMapped]
        public NflStatLine HomeStatLine
        {
            get
            {
                if (homeStatLine == null)
                {
                    homeStatLine = this.StatLines.First(x => x.Home);
                }

                return homeStatLine;
            }
        }

        [NotMapped]
        public String AwayTeam
        {
            get
            {
                return this.AwayStatLine.Participant.Name;
            }
        }

        [NotMapped]
        public String HomeTeam
        {
            get
            {
                return this.HomeStatLine.Participant.Name;
            }
        }

        [NotMapped]
        public Double AwayKnnPoints
        {
            get
            {
                return this.AwayStatLine.KnnPoints;
            }
        }

        [NotMapped]
        public Double HomeKnnPoints
        {
            get
            {
                return this.HomeStatLine.KnnPoints;
            }
        }
        #endregion
    }
}
