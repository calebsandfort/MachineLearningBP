using Abp.Domain.Services;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Core.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MachineLearningBP.Core.Services
{
    public interface ISheetUtilityDomainService : IDomainService
    {
        String GetAccessTokenUrl();
        void CompleteGetAccessToken(GetAccessTokenInput input);
        Task Record(List<IRecordContainer> results, [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "");
    }
}
