using MachineLearningBP.CollectiveIntelligence.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Services.Sports.Nba
{
    public interface INbaPointsExampleAppService : IExampleAppService
    {
        Task KNearestNeighborsDoStuff();
    }
}
