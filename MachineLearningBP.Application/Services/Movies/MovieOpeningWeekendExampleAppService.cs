using MachineLearningBP.CollectiveIntelligence.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Domain.Uow;

namespace MachineLearningBP.Services.Movies
{
    public class MovieOpeningWeekendExampleAppService : BaseApplicationService, IMovieOpeningWeekendExampleAppService
    {
        #region Properties
        private readonly IMovieOpeningWeekendExampleDomainService _movieOpeningWeekendExampleDomainService;
        private readonly ISheetUtilityAppService _sheetUtilityAppService;
        #endregion

        #region Ctor
        public MovieOpeningWeekendExampleAppService(ISettingManager settingManager, IMovieOpeningWeekendExampleDomainService movieOpeningWeekendExampleDomainService, ISheetUtilityAppService sheetUtilityAppService) : base(settingManager)
        {
            _movieOpeningWeekendExampleDomainService = movieOpeningWeekendExampleDomainService;
            _sheetUtilityAppService = sheetUtilityAppService;
        }
        #endregion

        #region PopulateExamples
        [UnitOfWork(IsDisabled = true)]
        public async Task PopulateExamples()
        {
            await this._movieOpeningWeekendExampleDomainService.PopulateExamples();
        }
        #endregion

        #region FindOptimalParametersPythonAndR
        [UnitOfWork(IsDisabled = true)]
        public async Task FindOptimalParametersPythonAndR()
        {
            await this._movieOpeningWeekendExampleDomainService.FindOptimalParametersPythonAndR(true);
        } 
        #endregion
    }
}
