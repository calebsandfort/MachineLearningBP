using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.GuerillaTimer;
using Abp.Configuration;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Entities.Sports.Nba;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.CollectiveIntelligence.DomainServices;

namespace MachineLearningBP.Core.Services.Sports.Nba
{
    public class NbaSeasonDomainService : MaximumSampleDomainService<NbaGame, NbaStatLine, NbaSeason, NbaTeam>, INbaSeasonDomainService
    {
        public NbaSeasonDomainService(IRepository<NbaTeam> participantRepository, IRepository<NbaSeason> timeGroupingRepository,
            IRepository<NbaGame> sampleRepository, IRepository<NbaStatLine> statLineRepository, ISqlExecuter sqlExecuter,
            IConsoleHubProxy consoleHubProxy, ISettingManager settingManager)
            : base(participantRepository, timeGroupingRepository, sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager)
        {
        }
        #region Constructor

        #endregion

        #region SetSeasonsRollingWindowStart
        public async Task SetSeasonsRollingWindowStart()
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    DateTime currentDate;
                    List<NbaSeason> seasons = this._timeGroupingRepository.GetAll().OrderBy(x => x.Start).ToList();
                    List<NbaTeam> teams = this._participantRepository.GetAllList();

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

            }
        }
        #endregion

        #region SetSeasonRollingWindowStart
        public bool SetSeasonRollingWindowStart(NbaSeason season, List<NbaTeam> teams, DateTime currentDate)
        {
            foreach (NbaTeam team in teams)
            {
                if (this._sampleRepository.Count(x => x.TimeGroupingId == season.Id && x.Date < currentDate && x.StatLines.Any(y => y.ParticipantId == team.Id)) < this._settingManager.GetSettingValue<int>("NbaRollingWindowPeriod"))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
