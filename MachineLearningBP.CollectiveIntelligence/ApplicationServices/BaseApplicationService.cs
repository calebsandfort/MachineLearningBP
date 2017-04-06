using Abp.Application.Services;
using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.ApplicationServices
{
    public class BaseApplicationService : ApplicationService
    {
        #region Properties
        public readonly ISettingManager _settingManager;
        #endregion

        #region Constructor
        public BaseApplicationService(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }
        #endregion
    }
}
