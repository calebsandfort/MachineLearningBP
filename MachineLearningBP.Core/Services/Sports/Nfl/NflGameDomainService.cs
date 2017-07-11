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
using MachineLearningBP.Entities.Sports.Nfl;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Samples;
using Abp.BackgroundJobs;
using MachineLearningBP.Entities.Sports.Nfl.Dtos;
using System.IO;
using CsvHelper;

namespace MachineLearningBP.Core.Services.Sports.Nfl
{
    public class NflGameDomainService : MaximumSampleDomainService<NflGame, NflStatLine, NflSeason, NflTeam>, INflGameDomainService
    {
        #region Properties
        private const String CoversUrlFormatter = "http://www.covers.com/sports/NFL/matchups?selectedDate={0:yyyy-MM-dd}";
        private const String PfrBoxScoreUrlFormatter = "http://www.pro-football-reference.com/boxscores/{0:yyyyMMdd}0{1}.htm";
        #endregion

        #region Contstructor
        public NflGameDomainService(IRepository<NflTeam> participantRepository, IRepository<NflSeason> timeGroupingRepository,
            IRepository<NflGame> sampleRepository, IRepository<NflStatLine> statLineRepository,
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
                DateTime now = DateTime.Now.Date;

                this.DeleteSamples();
                List<NflSeason> seasons = await this._timeGroupingRepository.GetAllListAsync();
                List<NflPlay> plays;

                foreach (NflSeason season in seasons.OrderBy(x => x.Start))
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Starting {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season..."));

                    if (season.Start < now) plays = GetPlaysForSeason(season);
                    else plays = new List<NflPlay>();

                    foreach (NflWeek nflWeek in season.Weeks)
                    {
                        await ScrapeGamesForDate(nflWeek.Start, now, season, plays);
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
                    NflSeason currentSeason;

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        currentDate = this._sampleRepository.GetAll().Where(x => x.Completed).Max(x => x.Date).AddDays(1);
                        currentSeason = this._timeGroupingRepository.GetAll().OrderByDescending(x => x.Start).First();
                        await unitOfWork.CompleteAsync();
                    }

                    while (currentDate <= now)
                    {
                        await ScrapeGamesForDate(currentDate, now, currentSeason, new List<NflPlay>());
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
        public async Task ScrapeGamesForDate(DateTime currentDate, DateTime now, NflSeason season, List<NflPlay> plays)
        {
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Scraping {currentDate.ToShortDateString()} ..."));

            List<HtmlNode> matchupBoxes;

            HtmlDocument doc = new HtmlDocument();
            HtmlWeb getHtml = new HtmlWeb();

            doc = getHtml.Load(String.Format(CoversUrlFormatter, currentDate));
            matchupBoxes = doc.DocumentNode.QuerySelectorAll("div.cmg_matchup_game_box").ToList();
            await Task.WhenAll(matchupBoxes.Select(x => ScrapeGame(x, currentDate, now, season, plays)).ToArray());

        }
        #endregion

        #region ScrapeGame
        public async Task ScrapeGame(HtmlNode matchupBox, DateTime currentDate, DateTime now, NflSeason season, List<NflPlay> plays)
        {
            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                try
                {
                    HtmlNode gameTime;
                    Double spread, total;
                    String awayTeamString, homeTeamString;
                    NflTeam awayTeam, homeTeam;
                    String[] teamNameSplit;

                    gameTime = matchupBox.QuerySelector(".cmg_game_time");
                    if (gameTime != null && gameTime.InnerText.Trim() == "Postponed")
                    {
                        return;
                    }

                    currentDate = matchupBox.QuerySelector(".cmg_matchup_header_date").InnerText.ScrapifyCoversDate(season.Start);

                    teamNameSplit = matchupBox.QuerySelector(".cmg_matchup_header_team_names").InnerText.Trim().Split(new String[] { " at ", " vs " }, StringSplitOptions.RemoveEmptyEntries);
                    awayTeamString = teamNameSplit[0];
                    homeTeamString = teamNameSplit[1];

                    awayTeam = await this._participantRepository.FirstOrDefaultAsync(x => x.Name == awayTeamString);
                    homeTeam = await this._participantRepository.FirstOrDefaultAsync(x => x.Name == homeTeamString);

                    if (awayTeam == null || homeTeam == null) return;

                    NflGame game = await this._sampleRepository.FirstOrDefaultAsync(x => x.StatLines.Any(y => y.Participant.Name == awayTeamString)
                        && x.StatLines.Any(y => y.Participant.Name == homeTeamString)
                        && x.Date.Year == currentDate.Year
                        && x.Date.Month == currentDate.Month
                        && x.Date.Day == currentDate.Day);

                    NflStatLine awayStatLine, homeStatLine;

                    if (game == null)
                    {
                        game = new NflGame();
                        game.Date = currentDate;

                        game.TimeGroupingId = season.Id;

                        //away basics
                        awayStatLine = new NflStatLine() { ParticipantId = awayTeam.Id, Home = false };

                        //home basics
                        homeStatLine = new NflStatLine() { ParticipantId = homeTeam.Id, Home = true };

                        game.StatLines = new List<NflStatLine>();
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
                        ScrapeStats(awayStatLine, homeStatLine, awayTeam, homeTeam, game, plays);
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

        #region ScrapeStats
        public void ScrapeStats(NflStatLine awayStatLine, NflStatLine homeStatLine, NflTeam awayTeam, NflTeam homeTeam, NflGame game, List<NflPlay> plays)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                HtmlWeb getHtml = new HtmlWeb();

                doc = getHtml.Load(String.Format(PfrBoxScoreUrlFormatter, game.Date, homeTeam.PfrId));
                doc.LoadHtml(doc.DocumentNode.OuterHtml.Replace("<!--", String.Empty).Replace("-->", String.Empty));

                List<HtmlNode> scoreDivs = doc.DocumentNode.QuerySelectorAll(".scorebox .score").ToList();
                homeStatLine.Points = scoreDivs[0].InnerText.ToDouble();
                awayStatLine.Points = scoreDivs[1].InnerText.ToDouble();

                HtmlNode teamStatsTable = doc.DocumentNode.QuerySelector("#team_stats");
                List<HtmlNode> teamStatsRows = teamStatsTable.Element("tbody").Elements("tr").ToList();

                #region Team Stats Table
                //Yards per play
                List<Double> rushAway = teamStatsRows[1].Elements("td").ElementAt(0).InnerText.Trim().Split("-".ToCharArray()).Select(x => Double.Parse(x)).ToList();
                List<Double> rushHome = teamStatsRows[1].Elements("td").ElementAt(1).InnerText.Trim().Split("-".ToCharArray()).Select(x => Double.Parse(x)).ToList();
                List<Double> passAway = teamStatsRows[2].Elements("td").ElementAt(0).InnerText.Trim().Split("-".ToCharArray()).Select(x => Double.Parse(x)).ToList();
                List<Double> passHome = teamStatsRows[2].Elements("td").ElementAt(1).InnerText.Trim().Split("-".ToCharArray()).Select(x => Double.Parse(x)).ToList();
                List<Double> sackAway = teamStatsRows[3].Elements("td").ElementAt(0).InnerText.Trim().Split("-".ToCharArray()).Select(x => Double.Parse(x)).ToList();
                List<Double> sackHome = teamStatsRows[3].Elements("td").ElementAt(1).InnerText.Trim().Split("-".ToCharArray()).Select(x => Double.Parse(x)).ToList();

                awayStatLine.TotalYards = teamStatsRows[4].Elements("td").ElementAt(0).InnerText.Trim().ToDouble();
                homeStatLine.TotalYards = teamStatsRows[4].Elements("td").ElementAt(1).InnerText.Trim().ToDouble();
                awayStatLine.TotalPlays = (rushAway[0] + passAway[1] + sackAway[0]);
                homeStatLine.TotalPlays = (rushHome[0] + passHome[1] + sackHome[0]);

                awayStatLine.YardsPerPlay = awayStatLine.TotalYards / awayStatLine.TotalPlays;
                homeStatLine.YardsPerPlay = homeStatLine.TotalYards / homeStatLine.TotalPlays;

                //Turnovers
                Double turnoversAway = teamStatsRows[7].Elements("td").ElementAt(0).InnerText.Trim().ToDouble();
                Double turnoversHome = teamStatsRows[7].Elements("td").ElementAt(1).InnerText.Trim().ToDouble();

                awayStatLine.Turnovers = turnoversAway;
                awayStatLine.TurnoversForced = turnoversHome;
                awayStatLine.TurnoverMargin = awayStatLine.TurnoversForced - awayStatLine.Turnovers;

                homeStatLine.Turnovers = turnoversHome;
                homeStatLine.TurnoversForced = turnoversAway;
                homeStatLine.TurnoverMargin = homeStatLine.TurnoversForced - homeStatLine.Turnovers;
                #endregion

                #region Drive Tables
                HtmlNode awayDrivesTable = doc.DocumentNode.QuerySelector("#vis_drives");
                HtmlNode homeDrivesTable = doc.DocumentNode.QuerySelector("#home_drives");

                Action<HtmlNode, NflStatLine, String> extractDriveStats = (driveTable, statLine, opp) =>
                {
                    List<HtmlNode> driveRows = driveTable.Element("tbody").Elements("tr").ToList();

                    List<HtmlNode> driveColumns = new List<HtmlNode>();
                    String[] losSplit;
                    Double driveStart;
                    Double driveFinish;
                    List<Double> driveStarts = new List<double>();
                    Double drivesInside40 = 0;
                    Double pointsOffDrivesInside40 = 0;

                    foreach (HtmlNode driveRow in driveRows)
                    {
                        driveColumns = driveRow.Elements("td").ToList();
                        losSplit = driveColumns[2].InnerText.Split(" ".ToCharArray());
                        if (losSplit.Length < 2) continue;

                        if(losSplit[0].ToLower() == opp.ToLower()) driveStart = 100 - Double.Parse(losSplit[1]);
                        else driveStart = Double.Parse(losSplit[1]);
                        
                        driveStarts.Add(driveStart);

                        driveFinish = driveStart + driveColumns[5].InnerText.ToDouble();
                        if(driveFinish >= 60)
                        {
                            drivesInside40 += 1.0;
                            if (driveColumns[6].InnerText == "Touchdown") pointsOffDrivesInside40 += 7;
                            if (driveColumns[6].InnerText == "Feld Goal") pointsOffDrivesInside40 += 3;
                        }
                    }

                    statLine.TotalDrives = driveStarts.Count();
                    statLine.FieldPosition = driveStarts.Average();

                    statLine.DrivesInside40 = drivesInside40;
                    statLine.PointsDrivesInside40 = pointsOffDrivesInside40;
                    statLine.PointsPerDriveInside40 = drivesInside40 > 0 ? (pointsOffDrivesInside40 / drivesInside40) : 0;
                };

                extractDriveStats(awayDrivesTable, awayStatLine, homeTeam.PfrId);
                extractDriveStats(homeDrivesTable, homeStatLine, awayTeam.PfrId);
                #endregion

                #region Success Rate
                int gameId = plays.First(x => x.GameDate.Year == game.Date.Year && x.GameDate.Month == game.Date.Month && x.GameDate.Day == game.Date.Day && x.OffenseTeam == awayTeam.SavId && x.DefenseTeam == homeTeam.SavId).GameId;
                List<NflPlay> gamePlays = plays.Where(x => x.GameId == gameId).ToList();

                Action<int, Double, NflTeam, NflTeam, NflStatLine> setSuccessValues = (down, neededThreshold, offTeam, defTeam, statLine) =>
                {
                    statLine.TotalSuccessRatePlays += gamePlays.Count(x => x.Down == down && x.OffenseTeam == offTeam.SavId && x.DefenseTeam == defTeam.SavId);
                    statLine.TotalSuccessfulSuccessRatePlays += gamePlays.Count(x => x.Down == down && x.OffenseTeam == offTeam.SavId && x.DefenseTeam == defTeam.SavId && x.Yards >= (x.ToGo * neededThreshold));
                };

                setSuccessValues(1, .5, awayTeam, homeTeam, awayStatLine);
                setSuccessValues(2, .7, awayTeam, homeTeam, awayStatLine);
                setSuccessValues(3, 1.0, awayTeam, homeTeam, awayStatLine);
                setSuccessValues(4, 1.0, awayTeam, homeTeam, awayStatLine);

                setSuccessValues(1, .5, homeTeam, awayTeam, homeStatLine);
                setSuccessValues(2, .7, homeTeam, awayTeam, homeStatLine);
                setSuccessValues(3, 1.0, homeTeam, awayTeam, homeStatLine);
                setSuccessValues(4, 1.0, homeTeam, awayTeam, homeStatLine);

                awayStatLine.SuccessRate = awayStatLine.TotalSuccessfulSuccessRatePlays / awayStatLine.TotalSuccessRatePlays;
                homeStatLine.SuccessRate = homeStatLine.TotalSuccessfulSuccessRatePlays / homeStatLine.TotalSuccessRatePlays;

                #endregion
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
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
                this._sqlExecuter.Execute("DELETE FROM [NflGames]");
                unitOfWork.Complete();
            }

            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting games finished."));
        }
        #endregion

        #region GetPlaysForSeason
        List<NflPlay> GetPlaysForSeason(NflSeason season)
        {
            try
            {
                using (TextReader reader = File.OpenText($"C:\\Users\\csandfort\\Documents\\Visual Studio 2017\\Projects\\MachineLearningBP\\MachineLearningBP.Core\\Files\\NFL\\pbp-{season.Start.Year}.csv"))
                {
                    var csv = new CsvReader(reader);
                    csv.Configuration.RegisterClassMap<NflPlayMap>();
                    return csv.GetRecords<NflPlay>().ToList();
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }

            return new List<NflPlay>();
        }
        #endregion
    }
}
