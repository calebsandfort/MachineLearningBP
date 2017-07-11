using MachineLearningBP.CollectiveIntelligence.DomainServices;
using MachineLearningBP.Core.Services;
using MachineLearningBP.Entities.Sports.Nfl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Core.Services.Sports.Nfl
{
    public interface INflGameDomainService : IGameDomainService<NflSeason, NflStatLine>
    {
    }
}
