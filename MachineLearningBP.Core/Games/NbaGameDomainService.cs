using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using MachineLearningBP.Framework;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Seasons;
using MachineLearningBP.StatLines;
using MachineLearningBP.Teams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.Shared.GuerillaTimer;
using Abp.Configuration;

namespace MachineLearningBP.Games
{
    public class NbaGameDomainService : NbaDomainService, INbaGameDomainService
    {
        #region Properties
        private const String CoversUrlFormatter = "http://www.covers.com/sports/NBA/matchups?selectedDate={0:yyyy-MM-dd}";
        #endregion

        public NbaGameDomainService(IRepository<NbaGame> nbaGameRepository, IRepository<NbaTeam> nbaTeamRepository, IRepository<NbaSeason> nbaSeasonRepository,
            IRepository<NbaStatLine> nbaStatLineRepository, ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IGuerillaTimer guerillaTimer,
            ISettingManager settingManager)
            : base(nbaGameRepository, nbaTeamRepository, nbaSeasonRepository, nbaStatLineRepository, sqlExecuter, consoleHubProxy, guerillaTimer, settingManager)
        {
        }

        #region Data Scraping
        #region PopulateGames
        public async Task PopulateGames()
        {
            this._guerillaTimer.Start("Populating games");

            DateTime currentDate;
            DateTime now = DateTime.Now.Date;

            this.DeleteGames();
            List<NbaSeason> seasons = await this._nbaSeasonRepository.GetAllListAsync();

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

            this._guerillaTimer.Complete();
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

                    awayTeam = await this._nbaTeamRepository.FirstOrDefaultAsync(x => x.CoversName == awayTeamString);
                    homeTeam = await this._nbaTeamRepository.FirstOrDefaultAsync(x => x.CoversName == homeTeamString);

                    if (awayTeam == null || homeTeam == null) return;

                    NbaGame game = await this._nbaGameRepository.FirstOrDefaultAsync(x => x.StatLines.Any(y => y.Team.CoversName == awayTeamString)
                        && x.StatLines.Any(y => y.Team.CoversName == homeTeamString)
                        && x.Date.Year == currentDate.Year
                        && x.Date.Month == currentDate.Month
                        && x.Date.Day == currentDate.Day);

                    NbaStatLine awayStatLine, homeStatLine;

                    if (game == null)
                    {
                        game = new NbaGame();
                        game.Date = currentDate;

                        game.SeasonId = season.Id;

                        //away basics
                        awayStatLine = new NbaStatLine() { TeamId = awayTeam.Id, Home = false };
                        await SetFrequencyProperties(awayStatLine, currentDate);

                        //home basics
                        homeStatLine = new NbaStatLine() { TeamId = homeTeam.Id, Home = true };
                        await SetFrequencyProperties(homeStatLine, currentDate);

                        game.StatLines = new List<NbaStatLine>();
                        game.StatLines.Add(awayStatLine);
                        game.StatLines.Add(homeStatLine);

                        await this._nbaGameRepository.InsertAsync(game);
                    }
                    else
                    {
                        awayStatLine = await this._nbaStatLineRepository.FirstOrDefaultAsync(x => x.GameId == game.Id && x.Home == false);
                        homeStatLine = await this._nbaStatLineRepository.FirstOrDefaultAsync(x => x.GameId == game.Id && x.Home == true);
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
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message}"));
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
            bool oneDayAgo = await this._nbaStatLineRepository.CountAsync(x => x.TeamId == nbaStatLine.TeamId
                                                        && x.Game.Date.Year == gameDate.Year
                                                        && x.Game.Date.Month == gameDate.Month
                                                        && x.Game.Date.Day == gameDate.Day) > 0;

            gameDate = gameDate.AddDays(-1);
            bool twoDaysAgo = await this._nbaStatLineRepository.CountAsync(x => x.TeamId == nbaStatLine.TeamId
                                                        && x.Game.Date.Year == gameDate.Year
                                                        && x.Game.Date.Month == gameDate.Month
                                                        && x.Game.Date.Day == gameDate.Day) > 0;

            gameDate = gameDate.AddDays(-1);
            bool threeDaysAgo = await this._nbaStatLineRepository.CountAsync(x => x.TeamId == nbaStatLine.TeamId
                                                        && x.Game.Date.Year == gameDate.Year
                                                        && x.Game.Date.Month == gameDate.Month
                                                        && x.Game.Date.Day == gameDate.Day) > 0;

            gameDate = gameDate.AddDays(-1);
            bool fourDaysAgo = await this._nbaStatLineRepository.CountAsync(x => x.TeamId == nbaStatLine.TeamId
                                                        && x.Game.Date.Year == gameDate.Year
                                                        && x.Game.Date.Month == gameDate.Month
                                                        && x.Game.Date.Day == gameDate.Day) > 0;

            gameDate = gameDate.AddDays(-1);
            bool fiveDaysAgo = await this._nbaStatLineRepository.CountAsync(x => x.TeamId == nbaStatLine.TeamId
                                                        && x.Game.Date.Year == gameDate.Year
                                                        && x.Game.Date.Month == gameDate.Month
                                                        && x.Game.Date.Day == gameDate.Day) > 0;

            gameDate = gameDate.AddDays(-1);
            bool sixDaysAgo = await this._nbaStatLineRepository.CountAsync(x => x.TeamId == nbaStatLine.TeamId
                                                        && x.Game.Date.Year == gameDate.Year
                                                        && x.Game.Date.Month == gameDate.Month
                                                        && x.Game.Date.Day == gameDate.Day) > 0;

            nbaStatLine.TwoInTwoDays = oneDayAgo;
            nbaStatLine.ThreeInFourDays = (oneDayAgo && threeDaysAgo) || (twoDaysAgo && threeDaysAgo);
            nbaStatLine.FourInFiveDays = (oneDayAgo && threeDaysAgo && fourDaysAgo);
            nbaStatLine.FourInSixDays = (twoDaysAgo && threeDaysAgo && fiveDaysAgo) || (twoDaysAgo && fourDaysAgo && fiveDaysAgo) || (oneDayAgo && fourDaysAgo && fiveDaysAgo);
            nbaStatLine.FiveInSevenDays = (oneDayAgo && threeDaysAgo && fourDaysAgo && sixDaysAgo) || (twoDaysAgo && threeDaysAgo && fiveDaysAgo && sixDaysAgo);
        }
        #endregion 
        #endregion

        #region DeleteGames
        public void DeleteGames()
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
