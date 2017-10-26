using MachineLearningBP.Services.iSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MachineLearningBP.Tests.iSupport
{
    public class Incident_Tests : MachineLearningBPTestBase
    {
        readonly IIncidentDomainService _incidentDomainService;
        readonly IIncidentExampleDomainService _incidentExampleDomainService;

        public Incident_Tests()
        {
            _incidentDomainService = Resolve<IIncidentDomainService>();
            _incidentExampleDomainService = Resolve<IIncidentExampleDomainService>();
        }

        [Fact(DisplayName = "Incident.PopulateSamples")]
        public async Task PopulateSamples()
        {
            await _incidentDomainService.PopulateSamples();
        }

        [Fact(DisplayName = "Incident.PopulateExamples")]
        public async Task PopulateExamples()
        {
            await _incidentExampleDomainService.PopulateExamples();
        }

        [Fact(DisplayName = "Incident.FindOptimalParametersPythonAndR")]
        public async Task FindOptimalParametersPythonAndR()
        {
            await _incidentExampleDomainService.FindOptimalParametersPythonAndR(true);
        }
    }
}
