using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.iSupport
{
    public class IncidentExampleGenerationInfo : ExampleGenerationInfo
    {
        #region Properties
        public Incident Incident { get; set; }
        public IncidentStatLine StatLine { get; set; }
        #endregion

        #region Ctor
        public IncidentExampleGenerationInfo(Incident Incident)
        {
            this.Incident = Incident;
            this.StatLine = Incident.StatLines.First();
        }
        #endregion
    }
}
