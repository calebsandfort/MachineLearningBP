using Abp.Domain.Repositories;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using MachineLearningBP.Framework;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MachineLearningBP.Shared.GuerillaTimer;
using Abp.Configuration;
using MachineLearningBP.Entities.Sports.Nba;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Samples;
using Abp.BackgroundJobs;

namespace MachineLearningBP.Core.Services.Sports.Nba
{
    public class NbaGameDomainService : MaximumSampleDomainService<NbaGame, NbaStatLine, NbaSeason, NbaTeam>, INbaGameDomainService
    {
        #region Properties
        private const String CoversUrlFormatter = "http://www.covers.com/sports/NBA/matchups?selectedDate={0:yyyy-MM-dd}";
        #endregion

        #region Contstructor
        public NbaGameDomainService(IRepository<NbaTeam> participantRepository, IRepository<NbaSeason> timeGroupingRepository,
            IRepository<NbaGame> sampleRepository, IRepository<NbaStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager)
            : base(participantRepository, timeGroupingRepository, sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
        {
        }
        #endregion

        #region Data Scraping
        #region PopulateSamples
        public async Task PopulateSamples()
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                DateTime currentDate;
                DateTime now = DateTime.Now.Date;

                this.DeleteSamples();
                List<NbaSeason> seasons = await this._timeGroupingRepository.GetAllListAsync();

                foreach (NbaSeason season in seasons.OrderBy(x => x.Start))
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Starting {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season..."));
                    currentDate = season.Start.Date;
                    while (season.Within(currentDate))
                    {
                        await ScrapeGamesForDate(currentDate, now, season);

                        currentDate = currentDate.AddDays(1);
                    }
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Completed {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season."));
                }
            }
        }
        #endregion

        #region FillGames
        public async Task FillSamples()
        {
            try
            {
                using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
                {

                    DateTime currentDate;
                    DateTime now = DateTime.Now.Date;
                    NbaSeason currentSeason;

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        currentDate = this._sampleRepository.GetAll().Where(x => x.Completed).Max(x => x.Date).AddDays(1);
                        currentSeason = this._timeGroupingRepository.GetAll().OrderByDescending(x => x.Start).First();
                        await unitOfWork.CompleteAsync();
                    }

                    while (currentDate <= now)
                    {
                        await ScrapeGamesForDate(currentDate, now, currentSeason);
                        currentDate = currentDate.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }
        }
        #endregion

        #region ScrapeGamesForDate
        public async Task ScrapeGamesForDate(DateTime currentDate, DateTime now, NbaSeason season)
        {
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Scraping {currentDate.ToShortDateString()} ..."));

            List<HtmlNode> matchupBoxes;

            HtmlDocument doc = new HtmlDocument();
            HtmlWeb getHtml = new HtmlWeb();

            doc = getHtml.Load(String.Format(CoversUrlFormatter, currentDate));
            matchupBoxes = doc.DocumentNode.QuerySelectorAll("div.cmg_matchup_game_box").ToList();
            await Task.WhenAll(matchupBoxes.Select(x => ScrapeGame(x, currentDate, now, season)).ToArray());

        }
        #endregion

        #region ScrapeGame
        public async Task ScrapeGame(HtmlNode matchupBox, DateTime currentDate, DateTime now, NbaSeason season)
        {
            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                try
                {
                    HtmlNode gameTime;
                    Double spread, total;
                    String awayTeamString, homeTeamString;
                    NbaTeam awayTeam, homeTeam;
                    String[] teamNameSplit;

                    gameTime = matchupBox.QuerySelector(".cmg_game_time");
                    if (gameTime != null && gameTime.InnerText.Trim() == "Postponed")
                    {
                        return;
                    }

                    teamNameSplit = matchupBox.QuerySelector(".cmg_matchup_header_team_names").InnerText.Trim().Split(new String[] { " at ", " vs " }, StringSplitOptions.RemoveEmptyEntries);
                    awayTeamString = teamNameSplit[0];
                    homeTeamString = teamNameSplit[1];

                    awayTeam = await this._participantRepository.FirstOrDefaultAsync(x => x.Name == awayTeamString);
                    homeTeam = await this._participantRepository.FirstOrDefaultAsync(x => x.Name == homeTeamString);

                    if (awayTeam == null || homeTeam == null) return;

                    NbaGame game = await this._sampleRepository.FirstOrDefaultAsync(x => x.StatLines.Any(y => y.Participant.Name == awayTeamString)
                        && x.StatLines.Any(y => y.Participant.Name == homeTeamString)
                        && x.Date.Year == currentDate.Year
                        && x.Date.Month == currentDate.Month
                        && x.Date.Day == currentDate.Day);

                    NbaStatLine awayStatLine, homeStatLine;

                    if (game == null)
                    {
                        game = new NbaGame();
                        game.Date = currentDate;

                        game.TimeGroupingId = season.Id;

                        //away basics
                        awayStatLine = new NbaStatLine() { ParticipantId = awayTeam.Id, Home = false };
                        await SetFrequencyProperties(awayStatLine, currentDate);

                        //home basics
                        homeStatLine = new NbaStatLine() { ParticipantId = homeTeam.Id, Home = true };
                        await SetFrequencyProperties(homeStatLine, currentDate);

                        game.StatLines = new List<NbaStatLine>();
                        game.StatLines.Add(awayStatLine);
                        game.StatLines.Add(homeStatLine);

                        await this._sampleRepository.InsertAsync(game);
                    }
                    else
                    {
                        awayStatLine = await this._statLineRepository.FirstOrDefaultAsync(x => x.SampleId == game.Id && x.Home == false);
                        homeStatLine = await this._statLineRepository.FirstOrDefaultAsync(x => x.SampleId == game.Id && x.Home == true);
                    }

                    game.Completed = currentDate < now;

                    if (currentDate <= now)
                    {
                        spread = Double.Parse(matchupBox.Attributes["data-game-odd"].Value);
                        total = Double.Parse(matchupBox.Attributes["data-game-total"].Value);

                        game.Spread = spread;
                        game.Total = total;
                    }

                    if (currentDate < now)
                    {
                        HtmlDocument doc = new HtmlDocument();
                        HtmlWeb getHtml = new HtmlWeb();

                        doc = getHtml.Load(matchupBox.QuerySelector(".cmg_matchup_list_gamebox a").Attributes["href"].Value);
                        List<HtmlNode> statTables = doc.DocumentNode.QuerySelectorAll(".num").Take(2).ToList();
                        ScrapeStatTable(awayStatLine, statTables[0].QuerySelectorAll(".datahl2b").Last());
                        ScrapeStatTable(homeStatLine, statTables[1].QuerySelectorAll(".datahl2b").Last());

                        //datahl2b
                    }
                    await unitOfWork.CompleteAsync();
                }
                catch (Exception ex)
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                }
            }
        }
        #endregion

        #region ScrapeStatTable
        public void ScrapeStatTable(NbaStatLine statLine, HtmlNode summaryRow)
        {
            List<HtmlNode> statColumns = summaryRow.QuerySelectorAll("td").ToList();

            List<Double> fieldGoals = statColumns[3].InnerText.SplitDoubles();
            statLine.FieldGoalsMade = fieldGoals[0];
            statLine.FieldGoalsAttempted = fieldGoals[1];

            List<Double> threePointers = statColumns[4].InnerText.SplitDoubles();
            statLine.ThreePointersMade = threePointers[0];
            statLine.ThreePointersAttempted = threePointers[1];

            List<Double> freeThrows = statColumns[5].InnerText.SplitDoubles();
            statLine.FreeThrowsMade = freeThrows[0];
            statLine.FreeThrowsAttempted = freeThrows[1];

            statLine.OffensiveRebounds = statColumns[7].InnerText.ToDouble();
            statLine.DefensiveRebounds = statColumns[8].InnerText.ToDouble();

            statLine.Turnovers = statColumns[13].InnerText.ToDouble();
            statLine.Points = statColumns[15].InnerText.ToDouble();
        }
        #endregion

        #region SetFrequencyProperties
        private async Task SetFrequencyProperties(NbaStatLine nbaStatLine, DateTime gameDate)
        {
            gameDate = gameDate.AddDays(-1);
            bool oneDayAgo = await this._statLineRepository.CountAsync(x => x.ParticipantId == nbaStatLine.ParticipantId
                                                        && x.Sample.Date.Year == gameDate.Year
                                                        && x.Sample.Date.Month == gameDate.Month
                                                        && x.Sample.Date.Day == gameDate.Day) > 0;

            gameDate = gameDate.AddDays(-1);
            bool twoDaysAgo = await this._statLineRepository.CountAsync(x => x.ParticipantId == nbaStatLine.ParticipantId
                                                        && x.Sample.Date.Year == gameDate.Year
                                                        && x.Sample.Date.Month == gameDate.Month
                                                        && x.Sample.Date.Day == gameDate.Day) > 0;

            gameDate = gameDate.AddDays(-1);
            bool threeDaysAgo = await this._statLineRepository.CountAsync(x => x.ParticipantId == nbaStatLine.ParticipantId
                                                        && x.Sample.Date.Year == gameDate.Year
                                                        && x.Sample.Date.Month == gameDate.Month
                                                        && x.Sample.Date.Day == gameDate.Day) > 0;

            gameDate = gameDate.AddDays(-1);
            bool fourDaysAgo = await this._statLineRepository.CountAsync(x => x.ParticipantId == nbaStatLine.ParticipantId
                                                        && x.Sample.Date.Year == gameDate.Year
                                                        && x.Sample.Date.Month == gameDate.Month
                                                        && x.Sample.Date.Day == gameDate.Day) > 0;

            gameDate = gameDate.AddDays(-1);
            bool fiveDaysAgo = await this._statLineRepository.CountAsync(x => x.ParticipantId == nbaStatLine.ParticipantId
                                                        && x.Sample.Date.Year == gameDate.Year
                                                        && x.Sample.Date.Month == gameDate.Month
                                                        && x.Sample.Date.Day == gameDate.Day) > 0;

            gameDate = gameDate.AddDays(-1);
            bool sixDaysAgo = await this._statLineRepository.CountAsync(x => x.ParticipantId == nbaStatLine.ParticipantId
                                                        && x.Sample.Date.Year == gameDate.Year
                                                        && x.Sample.Date.Month == gameDate.Month
                                                        && x.Sample.Date.Day == gameDate.Day) > 0;

            nbaStatLine.TwoInTwoDays = oneDayAgo;
            nbaStatLine.ThreeInFourDays = (oneDayAgo && threeDaysAgo) || (twoDaysAgo && threeDaysAgo);
            nbaStatLine.FourInFiveDays = (oneDayAgo && threeDaysAgo && fourDaysAgo);
            nbaStatLine.FourInSixDays = (twoDaysAgo && threeDaysAgo && fiveDaysAgo) || (twoDaysAgo && fourDaysAgo && fiveDaysAgo) || (oneDayAgo && fourDaysAgo && fiveDaysAgo);
            nbaStatLine.FiveInSevenDays = (oneDayAgo && threeDaysAgo && fourDaysAgo && sixDaysAgo) || (twoDaysAgo && threeDaysAgo && fiveDaysAgo && sixDaysAgo);
        }
        #endregion 
        #endregion

        #region DeleteSamples
        public void DeleteSamples()
        {
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting games..."));

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute("DELETE FROM [NbaGames]");
                unitOfWork.Complete();
            }

            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting games finished."));
        }
        #endregion
    }
}
