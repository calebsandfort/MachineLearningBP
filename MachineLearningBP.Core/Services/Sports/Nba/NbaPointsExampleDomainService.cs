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
    public class NbaPointsExampleDomainService : SportNumbersOnlyExampleDomainService<NbaGame, NbaStatLine, NbaPointsExample, NbaExampleGenerationInfo, NbaSeason, NbaTeam>, INbaPointsExampleDomainService
    {
        #region Constructor
        public NbaPointsExampleDomainService(IRepository<NbaGame> sampleRepository, IRepository<NbaStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager,
            IRepository<NbaPointsExample> exampleRepository, IRepository<NbaSeason> timeGroupingRepository,
            IRepository<NbaTeam> participantRepository) : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy,
                settingManager, exampleRepository, timeGroupingRepository, participantRepository)
        {
        }
        #endregion

        #region PopulateExamples
        public override async Task PopulateExamples(int rollingWindowPeriod, double scaleFactor)
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                this.DeleteExamples();

                DateTime currentDate = this._sampleRepository.GetAll().Where(x => x.Completed).OrderBy(x => x.Date).First().Date;
                List<NbaSeason> seasons;
                List<NbaTeam> teams;
                List<NbaGame> games, todaysGames;

                seasons = this._timeGroupingRepository.GetAll().OrderBy(x => x.Start).ToList();
                teams = await this._participantRepository.GetAllListAsync();


                foreach (NbaSeason season in seasons.OrderBy(x => x.Start))
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Starting {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season..."));

                    games = this._sampleRepository.GetAllIncluding(x => x.StatLines)
                            .Where(x => x.TimeGroupingId == season.Id).ToList();

                    if (currentDate < season.Start.Date) currentDate = season.RollingWindowStart.Value.Date;

                    while (season.Within(currentDate))
                    {
                        this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Populating {currentDate.ToShortDateString()} ..."));

                        todaysGames = this._sampleRepository.GetAll().Where(x => x.Date.Year == currentDate.Year && x.Date.Month == currentDate.Month && x.Date.Day == currentDate.Day).ToList();

                        await Task.WhenAll(todaysGames.Select(x => PopulateExample(x, games, teams, rollingWindowPeriod, scaleFactor)).ToArray());

                        currentDate = currentDate.AddDays(1);
                    }

                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Completed {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season."));
                }
            }
        }
        #endregion

        #region PopulateExample
        public override async Task PopulateExample(NbaGame game, List<NbaGame> games, List<NbaTeam> teams, int rollingWindowPeriod, double scaleFactor)
        {
            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                NbaExampleGenerationInfo awayInfo = new NbaExampleGenerationInfo(game, games, teams, false, rollingWindowPeriod, scaleFactor);
                NbaPointsExample awayExample = new NbaPointsExample();
                awayExample.SetFields(awayInfo);
                this._exampleRepository.Insert(awayExample);

                NbaExampleGenerationInfo homeInfo = new NbaExampleGenerationInfo(game, games, teams, true, rollingWindowPeriod, scaleFactor);
                NbaPointsExample homeExample = new NbaPointsExample();
                homeExample.SetFields(homeInfo);
                this._exampleRepository.Insert(homeExample);

                await unitOfWork.CompleteAsync();
            }
        }
        #endregion

        #region DeleteExamples
        public override void DeleteExamples()
        {
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting NbaPointsExamples..."));

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute($"DELETE FROM [NbaPointsExamples]");
                unitOfWork.Complete();
            }

            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Deleting NbaPointsExamples finished."));
        }
        #endregion
    }
}
