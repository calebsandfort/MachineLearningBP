using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaGames")]
    public class NbaGame : Sample, Game<NbaSeason, NbaStatLine>
    {
        [ForeignKey("SampleId")]
        public virtual ICollection<NbaStatLine> StatLines { get; set; }

        [ForeignKey("TimeGroupingId")]
        public virtual NbaSeason TimeGrouping { get; set; }
        public virtual int TimeGroupingId { get; set; }

        public DateTime Date { get; set; }
        public int EspnIdentifier { get; set; }
        public bool Completed { get; set; }
        public double Total { get; set; }

        public Double Spread { get; set; }

        #region Computed
        [NotMapped]
        NbaStatLine awayStatLine = null;
        [NotMapped]
        public NbaStatLine AwayStatLine
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
        NbaStatLine homeStatLine = null;
        [NotMapped]
        public NbaStatLine HomeStatLine
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
