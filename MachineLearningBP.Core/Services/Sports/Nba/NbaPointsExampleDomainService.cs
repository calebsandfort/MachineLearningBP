using MachineLearningBP.CollectiveIntelligence.DomainServices.Examples;
using MachineLearningBP.Entities.Sports.Nba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Domain.Repositories;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Shared.GuerillaTimer;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms;

namespace MachineLearningBP.Services.Sports.Nba
{
    public class NbaPointsExampleDomainService : MaximumExampleDomainService<NbaGame, NbaStatLine, NbaPointsExample, Double, NbaExampleGenerationInfo, NbaSeason, NbaTeam>, INbaPointsExampleDomainService
    {
        #region Properties
        public readonly IKNearestNeighborsDomainService<NbaPointsExample, NbaStatLine, Double> _kNearestNeighborsDomainService;
        #endregion

        #region Constructor
        public NbaPointsExampleDomainService(IRepository<NbaGame> sampleRepository, IRepository<NbaStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager,
            IRepository<NbaPointsExample> exampleRepository, IRepository<NbaSeason> timeGroupingRepository,
            IRepository<NbaTeam> participantRepository, IKNearestNeighborsDomainService<NbaPointsExample, NbaStatLine, Double> kNearestNeighborsDomainService) : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy,
                settingManager, exampleRepository, timeGroupingRepository, participantRepository)
        {
            this._kNearestNeighborsDomainService = kNearestNeighborsDomainService;
        }
        #endregion

        #region PopulateExamples
        public async Task PopulateExamples(int rollingWindowPeriod, double scaleFactor)
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                this.DeleteExamples();

                DateTime now = DateTime.Now.Date;
                DateTime currentDate;
                List<NbaSeason> seasons;
                List<NbaTeam> teams;
                List<NbaGame> games, todaysGames;

                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    currentDate = this._sampleRepository.GetAll().Where(x => x.Completed).OrderBy(x => x.Date).First().Date.AddDays(-1);
                    seasons = this._timeGroupingRepository.GetAll().OrderBy(x => x.Start).ToList();
                    teams = await this._participantRepository.GetAllListAsync();
                    await unitOfWork.CompleteAsync();
                }

                foreach (NbaSeason season in seasons.OrderBy(x => x.Start))
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Starting {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season..."));

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        games = this._sampleRepository.GetAllIncluding(x => x.StatLines)
                            .Where(x => x.TimeGroupingId == season.Id).ToList();
                        await unitOfWork.CompleteAsync();
                    }

                    if (currentDate < season.Start.Date) currentDate = season.RollingWindowStart.Value.Date;

                    while (season.Within(currentDate))
                    {
                        if (currentDate == now) break;

                        this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Populating {currentDate.ToShortDateString()} ..."));

                        using (var unitOfWork = this.UnitOfWorkManager.Begin())
                        {
                            todaysGames = this._sampleRepository.GetAllIncluding(x => x.StatLines).Where(x => x.Date.Year == currentDate.Year && x.Date.Month == currentDate.Month && x.Date.Day == currentDate.Day).ToList();
                            await unitOfWork.CompleteAsync();
                        }

                        await Task.WhenAll(todaysGames.Select(x => PopulateExample(x, games, teams, rollingWindowPeriod, scaleFactor)).ToArray());
                        currentDate = currentDate.AddDays(1);
                    }

                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Completed {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season."));
                }
            }
        }
        #endregion

        #region PopulateExample
        public async Task PopulateExample(NbaGame game, List<NbaGame> games, List<NbaTeam> teams, int rollingWindowPeriod, double scaleFactor)
        {
            try
            {
                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    NbaExampleGenerationInfo awayInfo = new NbaExampleGenerationInfo(game, games, teams, false, rollingWindowPeriod, scaleFactor);
                    NbaPointsExample awayExample = new NbaPointsExample();
                    awayExample.SetFields(awayInfo.TeamStatLine1, awayInfo);
                    await this._exampleRepository.InsertAsync(awayExample);

                    NbaExampleGenerationInfo homeInfo = new NbaExampleGenerationInfo(game, games, teams, true, rollingWindowPeriod, scaleFactor);
                    NbaPointsExample homeExample = new NbaPointsExample();
                    homeExample.SetFields(awayInfo.TeamStatLine1, homeInfo);
                    await this._exampleRepository.InsertAsync(homeExample);

                    await unitOfWork.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }
        #endregion

        #region DeleteExamples
        public void DeleteExamples()
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

        #region KNearestNeighborsDoStuff
        public async Task KNearestNeighborsDoStuff()
        {
            List<NbaPointsExample> data, trainSet, testSet;

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                data = await this._exampleRepository.GetAllListAsync();
                unitOfWork.Complete();
            }

            this._kNearestNeighborsDomainService.DivideData(data, out trainSet, out testSet);
        } 
        #endregion
    }
}
