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

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Samples
{
    public class MinimumSampleDomainService<TSample, TStatLine> : BaseDomainService
        where TSample : Sample
        where TStatLine : StatLine
    {
        #region Properties
        public readonly IRepository<TSample> _sampleRepository;
        public readonly IRepository<TStatLine> _statLineRepository;
        #endregion

        #region MinimumDomainService
        public MinimumSampleDomainService(IRepository<TSample> sampleRepository, IRepository<TStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager)
            : base(sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
        {
            this._sampleRepository = sampleRepository;
            this._statLineRepository = statLineRepository;
        } 
        #endregion
    }
}
