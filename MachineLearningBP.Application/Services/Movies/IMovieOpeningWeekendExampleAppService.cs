using MachineLearningBP.CollectiveIntelligence.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Services.Movies
{
    public interface IMovieOpeningWeekendExampleAppService : IExampleAppService
    {
        Task FindOptimalParametersPythonAndR();
    }
}
