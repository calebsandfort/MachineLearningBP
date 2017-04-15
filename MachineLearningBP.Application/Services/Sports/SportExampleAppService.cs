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
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;

namespace MachineLearningBP.Services.Sports
{
    public class SportExampleAppService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo, TTimeGrouping, TParticipant, TGeneticOptimizeInput, TAnnealingOptimizeInput> : BaseApplicationService
        where TSample : Sample
        where TStatLine : StatLine
        where TTimeGrouping : TimeGrouping
        where TExampleGenerationInfo : ExampleGenerationInfo
        where TExample : ExampleMinimum<TStatLine, TResult, TExampleGenerationInfo>
        where TParticipant : Participant
        where TGeneticOptimizeInput : IGeneticOptimizeInput
        where TAnnealingOptimizeInput : IAnnealingOptimizeInput
    {
        public readonly ISportExampleDomainService<TSample, TParticipant, TExample, TStatLine, TResult, TGeneticOptimizeInput, TAnnealingOptimizeInput> _domainService;

        public SportExampleAppService(ISettingManager settingManager, ISportExampleDomainService<TSample, TParticipant, TExample, TStatLine, TResult, TGeneticOptimizeInput, TAnnealingOptimizeInput> domainService) : base(settingManager)
        {
            this._domainService = domainService;
        }
    }
}
