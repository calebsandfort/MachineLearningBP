using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaPointsExamples")]
    public class NbaPointsExample : NbaFourFactorsExample<SportExampleGenerationInfo<NbaGame, NbaStatLine, NbaSeason, NbaTeam>>
    {
        public override void SetResult(SportExampleGenerationInfo<NbaGame, NbaStatLine, NbaSeason, NbaTeam> info)
        {
            this.Result = info.TeamStatLine1.Points;
        }
    }
}
