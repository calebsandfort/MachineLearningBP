using MachineLearningBP.CollectiveIntelligence.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Application.Services.Sports
{
    public interface IGameAppService : ISampleAppService
    {
        Task FillSamples();
    }
}
