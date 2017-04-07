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

namespace MachineLearningBP.CollectiveIntelligence.DomainServices
{
    public class MinimumExampleDomainService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo> : MinimumSampleDomainService<TSample, TStatLine>
        where TSample : Sample
        where TStatLine : StatLine
        where TExample : Example<TResult, TExampleGenerationInfo>
        where TExampleGenerationInfo : ExampleGenerationInfo
    {
        #region Properties
        public readonly IRepository<TExample> _exampleRepository;
        #endregion

        #region MinimumExampleDomainService
        public MinimumExampleDomainService(IRepository<TSample> sampleRepository, IRepository<TStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager,
            IRepository<TExample> exampleRepository)
            : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager)
        {
            this._exampleRepository = exampleRepository;
        }
        #endregion
    }
}
