using Abp.Configuration;
using Abp.Domain.Uow;
using MachineLearningBP.CollectiveIntelligence.ApplicationServices;
using MachineLearningBP.Entities.Sports;
using MachineLearningBP.Entities.Sports.Nba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Services.Sports.Nba
{
    public class NbaPointsExampleAppService : BaseApplicationService, INbaPointsExampleAppService
    {
        private readonly INbaPointsExampleDomainService _nbaPointsExampleDomainService;

        public NbaPointsExampleAppService(ISettingManager settingManager, INbaPointsExampleDomainService nbaPointsExampleDomainService) : base(settingManager)
        {
            _nbaPointsExampleDomainService = nbaPointsExampleDomainService;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task PopulateExamples()
        {
            await this._nbaPointsExampleDomainService.PopulateExamples(this._settingManager.GetSettingValue<int>("NbaRollingWindowPeriod"), this._settingManager.GetSettingValue<int>("NbaScaleFactor"));
        }
    }
}
