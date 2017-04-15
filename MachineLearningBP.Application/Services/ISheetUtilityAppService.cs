using Abp.Application.Services;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Core.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Services
{
    public interface ISheetUtilityAppService : IApplicationService
    {
        String GetAccessTokenUrl();
        void CompleteGetAccessToken(GetAccessTokenInput input);
    }
}
