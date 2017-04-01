using MachineLearningBP.StatLines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Teams
{
    [Table("NbaTeams")]
    public class NbaTeam : Team
    {
        public String BasketballReferenceAbbr { get; set; }

        [ForeignKey("TeamId")]
        public virtual ICollection<NbaStatLine> StatLines { get; set; }
    }
}
