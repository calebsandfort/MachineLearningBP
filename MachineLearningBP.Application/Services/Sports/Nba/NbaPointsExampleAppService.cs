﻿using Abp.Configuration;
using MachineLearningBP.Entities.Sports;
using MachineLearningBP.Entities.Sports.Nba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Services.Sports.Nba
{
    public class NbaPointsExampleAppService : SportExampleAppService<NbaGame, NbaStatLine, NbaPointsExample, Double, NbaExampleGenerationInfo, NbaSeason, NbaTeam>, INbaPointsExampleAppService
    {
        public NbaPointsExampleAppService(ISettingManager settingManager, SportExampleDomainService<NbaGame, NbaStatLine, NbaPointsExample, Double, NbaExampleGenerationInfo, NbaSeason, NbaTeam> domainService) : base(settingManager, domainService)
        {
        }

        public async Task PopulateExamples()
        {
            await this._domainService.PopulateExamples(this._settingManager.GetSettingValue<int>("NbaRollingWindowPeriod"), this._settingManager.GetSettingValue<int>("NbaScaleFactory"));
        }
    }
}