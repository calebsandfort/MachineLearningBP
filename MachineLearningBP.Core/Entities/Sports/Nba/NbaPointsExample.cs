using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaPointsExamples")]
    public class NbaPointsExample : NbaFourFactorsNumbersOnlyExample
    {
        public override void SetResult(NbaExampleGenerationInfo info)
        {
            this.Result = info.TeamStatLine1.Points;
        }
    }
}
