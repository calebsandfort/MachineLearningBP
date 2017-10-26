using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.iSupport
{
    [Table("IncidentStatLines")]
    public class IncidentStatLine : StatLine, StatLineMinimum<Incident>
    {
        public String Customer { get; set; }
        public String Company { get; set; }
        public String Source { get; set; }
        public String Template { get; set; }
        public int Month { get; set; }
        public int DayOfWeek { get; set; }
        public int Priority { get; set; }
        public String Assignee { get; set; }
        public String Status { get; set; }
        public String Category { get; set; }

        [NotMapped]
        public String Category1 { get; set; }
        [NotMapped]
        public String Category2 { get; set; }
        [NotMapped]
        public String Category3 { get; set; }
        [NotMapped]
        public String Category4 { get; set; }
        [NotMapped]
        public String Category5 { get; set; }

        public int TotalTimeWorked { get; set; }

        [ForeignKey("SampleId")]
        public Incident Sample { get; set; }
        public int SampleId { get; set; }
    }
}
