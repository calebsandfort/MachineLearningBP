using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Services;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices
{
    public class BaseDomainService : DomainService
    {
        #region Properties
        public readonly ISqlExecuter _sqlExecuter;
        public readonly IConsoleHubProxy _consoleHubProxy;
        public readonly ISettingManager _settingManager;
        public readonly IBackgroundJobManager _backgroundJobManager;
        #endregion

        #region Constructor
        public BaseDomainService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager)
        {
            _sqlExecuter = sqlExecuter;
            _consoleHubProxy = consoleHubProxy;
            _settingManager = settingManager;
            _backgroundJobManager = backgroundJobManager;
        }
        #endregion
    }
}
