using Abp.Domain.Services;
using MachineLearningBP.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MachineLearningBP.Games;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.GuerillaTimer;
using MachineLearningBP.StatLines;
using MachineLearningBP.Teams;
using Abp.Configuration;
using MachineLearningBP.Shared.Dtos;

namespace MachineLearningBP.Seasons
{
    public class NbaSeasonDomainService : NbaDomainService, INbaSeasonDomainService
    {
        #region Constructor
        public NbaSeasonDomainService(IRepository<NbaGame> nbaGameRepository, IRepository<NbaTeam> nbaTeamRepository, IRepository<NbaSeason> nbaSeasonRepository,
            IRepository<NbaStatLine> nbaStatLineRepository, ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IGuerillaTimer guerillaTimer,
            ISettingManager settingManager)
            : base(nbaGameRepository, nbaTeamRepository, nbaSeasonRepository, nbaStatLineRepository, sqlExecuter, consoleHubProxy, guerillaTimer, settingManager)
        {
        }
        #endregion

        #region SetSeasonsRollingWindowStart
        public async Task SetSeasonsRollingWindowStart()
        {
            this._guerillaTimer.Start("Setting seasons rolling start");

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                DateTime currentDate;
                List<NbaSeason> seasons = this._nbaSeasonRepository.GetAll().OrderBy(x => x.Start).ToList();
                List<NbaTeam> teams = this._nbaTeamRepository.GetAllList();

                foreach (NbaSeason season in seasons)
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Starting {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season..."));

                    currentDate = season.Start.Date;
                    while (season.Within(currentDate))
                    {
                        if (SetSeasonRollingWindowStart(season, teams, currentDate))
                        {
                            season.RollingWindowStart = currentDate;
                            Console.WriteLine(currentDate);
                            break;
                        }

                        currentDate = currentDate.AddDays(1);
                    }

                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Completed {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season."));
                }

                await unitOfWork.CompleteAsync();
            }

            this._guerillaTimer.Complete();
        }
        #endregion

        #region SetSeasonRollingWindowStart
        public bool SetSeasonRollingWindowStart(NbaSeason season, List<NbaTeam> teams, DateTime currentDate)
        {
            foreach (Team team in teams)
            {
                if (this._nbaGameRepository.Count(x => x.SeasonId == season.Id && x.Date < currentDate && x.StatLines.Any(y => y.TeamId == team.Id)) < this._settingManager.GetSettingValue<int>("RollingWindowPeriod"))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
