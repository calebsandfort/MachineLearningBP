using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using MachineLearningBP.Games;
using MachineLearningBP.Seasons;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.GuerillaTimer;
using MachineLearningBP.StatLines;
using MachineLearningBP.Teams;

namespace MachineLearningBP.Framework
{
    public class NbaDomainService : DomainService
    {
        #region Properties
        public readonly IRepository<NbaGame> _nbaGameRepository;
        public readonly IRepository<NbaTeam> _nbaTeamRepository;
        public readonly IRepository<NbaSeason> _nbaSeasonRepository;
        public readonly IRepository<NbaStatLine> _nbaStatLineRepository;
        public readonly ISqlExecuter _sqlExecuter;
        public readonly IConsoleHubProxy _consoleHubProxy;
        public readonly IGuerillaTimer _guerillaTimer;
        public readonly ISettingManager _settingManager;
        #endregion

        #region Constructor
        public NbaDomainService(IRepository<NbaGame> nbaGameRepository, IRepository<NbaTeam> nbaTeamRepository,
            IRepository<NbaSeason> nbaSeasonRepository, IRepository<NbaStatLine> nbaStatLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IGuerillaTimer guerillaTimer,
            ISettingManager settingManager)
        {
            _nbaGameRepository = nbaGameRepository;
            _nbaTeamRepository = nbaTeamRepository;
            _nbaSeasonRepository = nbaSeasonRepository;
            _nbaStatLineRepository = nbaStatLineRepository;
            _sqlExecuter = sqlExecuter;
            _consoleHubProxy = consoleHubProxy;
            _guerillaTimer = guerillaTimer;
            _settingManager = settingManager;
        }
        #endregion
    }
}
