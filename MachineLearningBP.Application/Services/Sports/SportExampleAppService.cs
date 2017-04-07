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
    public class SportExampleAppService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo, TTimeGrouping, TParticipant> : BaseApplicationService
        where TSample : Sample
        where TStatLine : StatLine
        where TTimeGrouping : TimeGrouping
        where TExampleGenerationInfo : ExampleGenerationInfo
        where TExample : Example<TResult, TExampleGenerationInfo>
        where TParticipant : Participant
    {
        public readonly SportExampleDomainService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo, TTimeGrouping, TParticipant> _domainService;

        public SportExampleAppService(ISettingManager settingManager, SportExampleDomainService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo, TTimeGrouping, TParticipant> domainService) : base(settingManager)
        {
            this._domainService = domainService;
        }
    }
}
