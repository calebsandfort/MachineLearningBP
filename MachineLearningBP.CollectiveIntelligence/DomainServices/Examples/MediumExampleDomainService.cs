using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using Abp.Domain.Repositories;
using MachineLearningBP.CollectiveIntelligence.Entities;
using Abp.BackgroundJobs;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Examples
{
    public class MediumExampleDomainService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo, TTimeGrouping> : MinimumExampleDomainService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo>
        where TSample : Sample
        where TStatLine : StatLine
        where TExample : ExampleMinimum<TStatLine, TResult, TExampleGenerationInfo>
        where TExampleGenerationInfo : ExampleGenerationInfo
        where TTimeGrouping : TimeGrouping
    {
        #region Properties
        public readonly IRepository<TTimeGrouping> _timeGroupingRepository;
        #endregion

        #region Constructor
        public MediumExampleDomainService(IRepository<TSample> sampleRepository, IRepository<TStatLine> statLineRepository, ISqlExecuter sqlExecuter,
            IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IRepository<TExample> exampleRepository, IRepository<TTimeGrouping> timeGroupingRepository, IBackgroundJobManager backgroundJobManager)
            : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, exampleRepository, backgroundJobManager)
        {
            this._timeGroupingRepository = timeGroupingRepository;
        }
        #endregion
    }
}
