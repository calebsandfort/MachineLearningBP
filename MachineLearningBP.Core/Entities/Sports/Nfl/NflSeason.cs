using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Nfl
{
    [Table("NflSeasons")]
    public class NflSeason : TimeGrouping, Season<NflGame>
    {
        [ForeignKey("TimeGroupingId")]
        public virtual ICollection<NflGame> Samples { get; set; }

        public DateTime? RollingWindowStart { get; set; }

        [NotMapped]
        private List<NflWeek> _weeks = new List<NflWeek>();
        [NotMapped]
        public List<NflWeek> Weeks
        {
            get
            {
                if(this._weeks.Count == 0)
                {
                    for(int i = 0; i < 17; i++)
                    {
                        this._weeks.Add(new NflWeek { Start = this.Start.AddDays(i * 7) });
                        this._weeks[i].End = this._weeks[i].Start.AddDays(6);
                    }
                }

                return this._weeks;
            }
        }
    }
}
