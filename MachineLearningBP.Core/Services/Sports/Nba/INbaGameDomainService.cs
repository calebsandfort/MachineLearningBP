using MachineLearningBP.CollectiveIntelligence.DomainServices;
using MachineLearningBP.Core.Services;
using MachineLearningBP.Entities.Sports.Nba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Core.Services.Sports.Nba
{
    public interface INbaGameDomainService : IGameDomainService<NbaSeason, NbaStatLine>
    {
    }
}
