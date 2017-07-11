using Abp.Domain.Repositories;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MachineLearningBP.Shared.GuerillaTimer;
using Abp.Configuration;
using MachineLearningBP.Entities.Sports.Mlb;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Samples;
using System.Text.RegularExpressions;
using Abp.BackgroundJobs;
using MachineLearningBP.Entities.Sports.Nfl.Dtos;

namespace MachineLearningBP.Core.Services.Sports.Mlb
{
    public class MlbGameDomainService : MaximumSampleDomainService<MlbGame, MlbStatLine, MlbSeason, MlbTeam>, IMlbGameDomainService
    {
        #region Properties
        private const String CoversUrlFormatter = "http://www.covers.com/sports/Mlb/matchups?selectedDate={0:yyyy-MM-dd}";
        private const String CoversBoxScoreFormatter = "http://www.covers.com/pageLoader/pageLoader.aspx?page=/data/mlb/results/{0}/boxscore{1}.html";
        protected Regex SfRegEx { get; set; }
        protected Regex InningsPitchedRegEx { get; set; }
        #endregion

        #region Contstructor
        public MlbGameDomainService(IRepository<MlbTeam> participantRepository, IRepository<MlbSeason> timeGroupingRepository,
            IRepository<MlbGame> sampleRepository, IRepository<MlbStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager)
            : base(participantRepository, timeGroupingRepository, sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
        {
            this.SfRegEx = new Regex(@"\((\d+)\)", RegexOptions.Compiled);
            this.InningsPitchedRegEx = new Regex(@"\d+\.(\d+)", RegexOptions.Compiled);
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
                List<MlbSeason> seasons = await this._timeGroupingRepository.GetAllListAsync();

                foreach (MlbSeason season in seasons.OrderBy(x => x.Start))
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
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {

                DateTime currentDate;
                DateTime now = DateTime.Now.Date;
                MlbSeason currentSeason;

                currentDate = this._sampleRepository.GetAll().Where(x => x.Completed).Max(x => x.Date).AddDays(1);
                currentSeason = this._timeGroupingRepository.GetAll().OrderByDescending(x => x.Start).First();

                while (currentDate <= now)
                {
                    await ScrapeGamesForDate(currentDate, now, currentSeason);
                    currentDate = currentDate.AddDays(1);
                }
            }
        }
        #endregion

        #region ScrapeGamesForDate
        public async Task ScrapeGamesForDate(DateTime currentDate, DateTime now, MlbSeason season, List<NflPlay> plays = null)
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
        public async Task ScrapeGame(HtmlNode matchupBox, DateTime currentDate, DateTime now, MlbSeason season, List<NflPlay> plays = null)
        {
            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                try
                {
                    HtmlNode gameTimeNode;
                    int coversId;
                    String awayTeamString, homeTeamString;
                    MlbTeam awayTeam, homeTeam;
                    String[] teamNameSplit;
                    DateTime gameTime;

                    coversId = Int32.Parse(matchupBox.Attributes["data-event-id"].Value);

                    gameTimeNode = matchupBox.QuerySelector(".cmg_game_time");
                    if (gameTimeNode != null && gameTimeNode.InnerText.Trim() == "Postponed")
                    {
                        return;
                    }

                    teamNameSplit = matchupBox.QuerySelector(".cmg_matchup_header_team_names").InnerText.Trim().Split(new String[] { " at ", " vs " }, StringSplitOptions.RemoveEmptyEntries);
                    awayTeamString = teamNameSplit[0];
                    homeTeamString = teamNameSplit[1];

                    awayTeam = await this._participantRepository.FirstOrDefaultAsync(x => x.Name == awayTeamString);
                    homeTeam = await this._participantRepository.FirstOrDefaultAsync(x => x.Name == homeTeamString);

                    if (awayTeam == null || homeTeam == null) return;

                    MlbGame game = await this._sampleRepository.FirstOrDefaultAsync(x => x.CoversId == coversId);

                    MlbStatLine awayStatLine, homeStatLine;

                    if (game == null)
                    {
                        game = new MlbGame();
                        game.CoversId = coversId;
                        game.Date = DateTime.TryParse(matchupBox.Attributes["data-game-date"].Value, out gameTime) ? gameTime : currentDate;

                        game.TimeGroupingId = season.Id;

                        //away basics
                        awayStatLine = new MlbStatLine() { ParticipantId = awayTeam.Id, Home = false };

                        //home basics
                        homeStatLine = new MlbStatLine() { ParticipantId = homeTeam.Id, Home = true };

                        game.StatLines = new List<MlbStatLine>();
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

                    //TODO: Update to get current days odds
                    if (currentDate < now)
                    {
                        HtmlDocument doc = new HtmlDocument();
                        HtmlWeb getHtml = new HtmlWeb();
                        doc = getHtml.Load(String.Format(CoversBoxScoreFormatter, currentDate.Year, game.CoversId));

                        if (currentDate < now)
                        {
                            List<HtmlNode> statTables = doc.DocumentNode.QuerySelectorAll(".num").ToList();

                            ScrapeStatTable(awayStatLine, statTables[0], statTables[1]);
                            ScrapeStatTable(homeStatLine, statTables[2], statTables[3]);
                        }

                        List<HtmlNode> lineRows = matchupBox.QuerySelectorAll(".cmg_matchup_line_score tbody > tr").ToList();
                        List<HtmlNode> columns = lineRows[0].QuerySelectorAll("td").ToList();
                        awayStatLine.Moneyline = Double.Parse(columns[columns.Count - 4].InnerText);
                        columns = lineRows[1].QuerySelectorAll("td").ToList();
                        homeStatLine.Moneyline = Double.Parse(columns[columns.Count - 4].InnerText);

                        if(columns.Count < 5)
                        {
                            int t = 0;
                        }

                        if (columns[columns.Count - 3].InnerText.Trim() == "Off")
                            game.Total = awayStatLine.Points + homeStatLine.Points;
                        else
                            game.Total = Double.Parse(columns[columns.Count - 3].InnerText);
                    }
                    else if(currentDate.Date == now.Date)
                    {
                        try
                        {
                            HtmlNode awayOdds = matchupBox.QuerySelector(".cmg_matchup_list_away_odds");
                            awayStatLine.Moneyline = Double.Parse(awayOdds.ChildNodes[2].InnerText.Trim());

                            HtmlNode homeOdds = matchupBox.QuerySelector(".cmg_matchup_list_home_odds");
                            homeStatLine.Moneyline = Double.Parse(homeOdds.ChildNodes[0].InnerText.Trim());

                            game.Total = Double.Parse(matchupBox.QuerySelectorAll(".cmg_team_live_odds > span").ToList()[1].InnerText.Trim().Replace("O/U: ", String.Empty).Replace(" |", String.Empty));
                        }
                        catch { }
                    }

                    await unitOfWork.CompleteAsync();
                }
                catch (Exception ex)
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                    throw ex;
                }
            }
        }
        #endregion

        #region ScrapeStatTable
        public void ScrapeStatTable(MlbStatLine statLine, HtmlNode hittingTable, HtmlNode pitchingTable)
        {
            try
            {
                List<HtmlNode> columns;
                HtmlNode strongNode;

                List<HtmlNode> tableRows = hittingTable.QuerySelectorAll("tr").ToList();
                foreach (HtmlNode hittingTableRow in tableRows)
                {
                    columns = hittingTableRow.QuerySelectorAll("td").ToList();
                    strongNode = columns[0].QuerySelector("strong");

                    if (columns[0].InnerText == "Totals")
                    {
                        statLine.AtBats = Double.Parse(columns[1].InnerText);
                        statLine.Points = Double.Parse(columns[2].InnerText);
                        statLine.Hits = Double.Parse(columns[3].InnerText);
                        statLine.Walks = Double.Parse(columns[5].InnerText);
                    }
                    else if (strongNode != null && strongNode.InnerText.StartsWith("SF - ", StringComparison.CurrentCulture))
                    {
                        String[] sfSplit = columns[0].InnerText.Split(",".ToCharArray());
                        foreach (String sfString in sfSplit)
                        {
                            MatchCollection mc = this.SfRegEx.Matches(sfString.Trim());
                            if (mc[0].Success && mc[0].Groups.Count == 2)
                                statLine.SacrificeFlies += Double.Parse(mc[0].Groups[1].Value);
                        }
                    }
                }

                Double tempPartialInningsPitched = 0;
                Double partialInningsPitched = 0;
                Double inningsPitched = 0;

                tableRows = pitchingTable.QuerySelectorAll("tr").ToList();
                foreach (HtmlNode pitchingTableRow in tableRows)
                {
                    columns = pitchingTableRow.QuerySelectorAll("td").ToList();
                    strongNode = columns[0].QuerySelector("strong");

                    if (pitchingTableRow.Attributes.Any(x => x.Name == "class") && pitchingTableRow.Attributes["class"].Value == "datarow")
                    {
                        inningsPitched = Double.Parse(columns[1].InnerText);
                        tempPartialInningsPitched = 0;
                        MatchCollection mc = this.InningsPitchedRegEx.Matches(columns[1].InnerText.Trim());
                        if (mc[0].Success && mc[0].Groups.Count == 2)
                            tempPartialInningsPitched += Double.Parse(mc[0].Groups[1].Value);

                        partialInningsPitched += tempPartialInningsPitched;
                        inningsPitched -= (tempPartialInningsPitched / 10);

                        statLine.InningsPitched += inningsPitched;
                    }
                    else if (strongNode != null && strongNode.InnerText.StartsWith("HBP -", StringComparison.CurrentCulture))
                    {
                        statLine.HitByPitch = columns[0].InnerText.Split(",".ToCharArray()).Count();
                    }
                }

                statLine.InningsPitched += (partialInningsPitched / 3);
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }
        #endregion
        #endregion

        #region DeleteSamples
        public void DeleteSamples()
        {
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting games..."));

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute("DELETE FROM [MlbGames]");
                unitOfWork.Complete();
            }

            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting games finished."));
        }
        #endregion

        #region IGameDomainService<MlbSeason, MlbStatLine>.ScrapeStatTable(MlbStatLine statLine, HtmlNode summaryRow)
        public void ScrapeStatTable(MlbStatLine statLine, HtmlNode summaryRow)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
