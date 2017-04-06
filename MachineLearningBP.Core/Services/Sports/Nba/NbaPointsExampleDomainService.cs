using MachineLearningBP.CollectiveIntelligence.DomainServices;
using MachineLearningBP.Entities.Sports.Nba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Domain.Repositories;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Shared.GuerillaTimer;
using MachineLearningBP.Entities.Sports;

namespace MachineLearningBP.Services.Sports.Nba
{
    public class NbaPointsExampleDomainService : SportNumbersOnlyExampleDomainService<NbaGame, NbaStatLine, NbaPointsExample,
        SportExampleGenerationInfo<NbaGame, NbaStatLine, NbaSeason, NbaTeam>, NbaSeason, NbaTeam>, INbaPointsExampleDomainService
    {
        #region Constructor
        public NbaPointsExampleDomainService(IRepository<NbaGame> sampleRepository, IRepository<NbaStatLine> statLineRepository, ISqlExecuter sqlExecuter,
            IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IRepository<NbaPointsExample> exampleRepository,
            IRepository<NbaSeason> timeGroupingRepository, IRepository<NbaTeam> participantRepository) :
            base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, exampleRepository, timeGroupingRepository, participantRepository)
        {
        }
        #endregion

        #region DeleteExamples
        public override void DeleteExamples()
        {
            base.DeleteExamples("NbaPointsExamples");
        } 
        #endregion
    }
}
