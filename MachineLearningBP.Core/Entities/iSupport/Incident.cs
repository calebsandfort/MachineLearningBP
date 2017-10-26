using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.Framework;

namespace MachineLearningBP.Entities.iSupport
{
    [Table("Incidents")]
    public class Incident : Sample, SampleMinimum<IncidentStatLine>, IDbLoadable
    {
        public int iSupportId { get; set; }

        [NotMapped]
        public int LastId
        {
            get
            {
                return this.iSupportId;
            }
        }

        [ForeignKey("SampleId")]
        public virtual ICollection<IncidentStatLine> StatLines { get; set; }

        #region Static Methods
        public static List<Incident> GetListFromDataReader(IDataReader reader)
        {
            //public String Customer { get; set; }
            //public String Company { get; set; }
            //public String Source { get; set; }
            //public String Template { get; set; }
            //public int Month { get; set; }
            //public int DayOfWeek { get; set; }
            //public int Priority { get; set; }
            //public String Assignee { get; set; }
            //public String Status { get; set; }
            //public String Category1 { get; set; }
            //public String Category2 { get; set; }
            //public String Category3 { get; set; }
            //public String Category4 { get; set; }
            //public String Category5 { get; set; }
            //public int TotalTimeWorked { get; set; }

            List<Incident> incidents = reader.Select(x => new Incident
            {
                iSupportId = x.LoadValueFromReader("ID", 0),
                StatLines = new List<IncidentStatLine>()
                {
                    new IncidentStatLine
                    {
                        Customer = x.LoadValueFromReader("Customer", String.Empty),
                        Company = x.LoadValueFromReader("Company", String.Empty),
                        Source = x.LoadValueFromReader("Source", String.Empty),
                        Template = x.LoadValueFromReader("Template", String.Empty),
                        Month = x.LoadValueFromReader("Month", -1),
                        DayOfWeek = x.LoadValueFromReader("DayOfWeek", -1),
                        Priority = x.LoadValueFromReader("Priority", -1),
                        Status = x.LoadValueFromReader("Status", String.Empty),
                        Assignee = x.LoadValueFromReader("Assignee", String.Empty),
                        Category = x.LoadValueFromReader("Category", String.Empty),
                        TotalTimeWorked = x.LoadValueFromReader("TotalTimeWorked", 0)
                    }
                }
            }).ToList();

            return incidents;
        } 
        #endregion
    }
}
