﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.GuerillaTimer;
using Abp.Configuration;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Entities.Sports.Nfl;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Samples;
using Abp.BackgroundJobs;

namespace MachineLearningBP.Core.Services.Sports.Nfl
{
    public class NflSeasonDomainService : MaximumSampleDomainService<NflGame, NflStatLine, NflSeason, NflTeam>, INflSeasonDomainService
    {
        public NflSeasonDomainService(IRepository<NflTeam> participantRepository, IRepository<NflSeason> timeGroupingRepository,
            IRepository<NflGame> sampleRepository, IRepository<NflStatLine> statLineRepository, ISqlExecuter sqlExecuter,
            IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager)
            : base(participantRepository, timeGroupingRepository, sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
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
                    List<NflSeason> seasons = this._timeGroupingRepository.GetAll().OrderBy(x => x.Start).ToList();
                    List<NflTeam> teams = this._participantRepository.GetAllList();

                    foreach (NflSeason season in seasons)
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
        public bool SetSeasonRollingWindowStart(NflSeason season, List<NflTeam> teams, DateTime currentDate)
        {
            foreach (NflTeam team in teams)
            {
                if (this._sampleRepository.Count(x => x.TimeGroupingId == season.Id && x.Date < currentDate && x.StatLines.Any(y => y.ParticipantId == team.Id)) < this._settingManager.GetSettingValue<int>("NflRollingWindowPeriod"))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
