﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using Abp.Domain.Repositories;
using MachineLearningBP.CollectiveIntelligence.Entities;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices
{
    public class MaximumExampleDomainService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo, TTimeGrouping, TParticipant> : MediumExampleDomainService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo, TTimeGrouping>
        where TSample : Sample
        where TStatLine : StatLine
        where TExample : Example<TResult, TExampleGenerationInfo>
        where TExampleGenerationInfo : ExampleGenerationInfo
        where TTimeGrouping : TimeGrouping
        where TParticipant : Participant
    {
        #region Properties
        public readonly IRepository<TParticipant> _participantRepository;
        #endregion

        #region Constructor
        public MaximumExampleDomainService(IRepository<TSample> sampleRepository, IRepository<TStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager,
            IRepository<TExample> exampleRepository, IRepository<TTimeGrouping> timeGroupingRepository,
            IRepository<TParticipant> participantRepository)
            : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, exampleRepository, timeGroupingRepository)
        {
            this._participantRepository = participantRepository;
        }
        #endregion
    }
}
