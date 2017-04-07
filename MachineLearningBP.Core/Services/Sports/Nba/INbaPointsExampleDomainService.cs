using MachineLearningBP.CollectiveIntelligence.DomainServices;
using MachineLearningBP.Entities.Sports.Nba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Services.Sports.Nba
{
    public interface INbaPointsExampleDomainService : ISportExampleDomainService<NbaGame, NbaTeam>
    {
    }
}
