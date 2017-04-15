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
    public class MediumSampleDomainService<TSample, TStatLine, TTimeGrouping> : MinimumSampleDomainService<TSample, TStatLine>
        where TSample : Sample
        where TStatLine : StatLine
        where TTimeGrouping : TimeGrouping
    {
        #region Properties
        public readonly IRepository<TTimeGrouping> _timeGroupingRepository;
        #endregion

        #region Constructor
        public MediumSampleDomainService(IRepository<TTimeGrouping> timeGroupingRepository,
            IRepository<TSample> sampleRepository, IRepository<TStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager)
            : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
        {
            this._timeGroupingRepository = timeGroupingRepository;
        }
        #endregion


    }
}
