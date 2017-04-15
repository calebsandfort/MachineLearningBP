using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Core.Services;
using MachineLearningBP.Core.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MachineLearningBP.Services
{
    public class SheetUtilityAppService : ISheetUtilityAppService
    {
        #region Properties
        private readonly ISheetUtilityDomainService _sheetUtilityDomainService;
        #endregion

        #region Constructor
        public SheetUtilityAppService(ISheetUtilityDomainService sheetUtilityDomainService)
        {
            _sheetUtilityDomainService = sheetUtilityDomainService;
        }
        #endregion

        #region GetAccessToken
        public String GetAccessTokenUrl()
        {
            return this._sheetUtilityDomainService.GetAccessTokenUrl();
        }
        #endregion

        #region CompleteGetAccessToken
        public void CompleteGetAccessToken(GetAccessTokenInput input)
        {
            this._sheetUtilityDomainService.CompleteGetAccessToken(input);
        } 
        #endregion      
    }
}
