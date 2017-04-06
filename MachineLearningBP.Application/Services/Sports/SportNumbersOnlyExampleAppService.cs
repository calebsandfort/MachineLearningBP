using Abp.Application.Services;
using MachineLearningBP.CollectiveIntelligence.ApplicationServices;
using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Entities.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;

namespace MachineLearningBP.Services.Sports
{
    public class SportNumbersOnlyExampleAppService<TSample, TStatLine, TExample, TExampleGenerationInfo, TTimeGrouping, TParticipant> : BaseApplicationService
        where TSample : Game<TStatLine, TTimeGrouping>
        where TStatLine : SportStatLine<TStatLine, TTimeGrouping>
        where TExample : SportNumbersOnlyExample<TExampleGenerationInfo, TSample, TStatLine, TTimeGrouping, TParticipant>
        where TExampleGenerationInfo : SportExampleGenerationInfo<TSample, TStatLine, TTimeGrouping, TParticipant>
        where TTimeGrouping : Season<TStatLine, TTimeGrouping>
        where TParticipant : Participant
    {
        public readonly SportNumbersOnlyExampleDomainService<TSample, TStatLine, TExample, TExampleGenerationInfo, TTimeGrouping, TParticipant> _domainService;

        public SportNumbersOnlyExampleAppService(ISettingManager settingManager, SportNumbersOnlyExampleDomainService<TSample, TStatLine, TExample, TExampleGenerationInfo, TTimeGrouping, TParticipant> domainService) : base(settingManager)
        {
            this._domainService = domainService;
        }
    }
}
