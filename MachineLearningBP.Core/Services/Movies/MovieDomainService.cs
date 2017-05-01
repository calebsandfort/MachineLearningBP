using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Repositories;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Samples;
using MachineLearningBP.Entities.Movies;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Shared.GuerillaTimer;
using System.Collections.Generic;
using System.Linq;
using System;
using MachineLearningBP.Framework;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Abp.ObjectMapping;
using Abp.AutoMapper;
using System.Globalization;
using MachineLearningBP.Entities.Movies.Enums;
using MachineLearningBP.Shared.InflationCalculator;
using Abp.Timing;

namespace MachineLearningBP.Core.Services.Movies
{
    public class MovieDomainService : MediumSampleDomainService<Movie, MovieStatLine, MovieYear>, IMovieDomainService
    {
        #region Properties
        readonly IObjectMapper _objectMapper;
        readonly IInflationCalculator _inflationCalculator;
        private const String YearlyWideReleasesUrlFormatter = "http://www.boxofficemojo.com/yearly/chart/?page={0}&view=widedate&view2=domestic&yr={1:yyyy}&p=.htm";
        private const String MovieUrlFormatter = "http://www.boxofficemojo.com/movies/?id={0}";
        private const String MovieWeekendUrlFormatter = "http://www.boxofficemojo.com/movies/?page=weekend&id={0}";
        private Object movieLock = new Object();

        List<String> genres = new List<string>();
        List<String> ratings = new List<string>();
        List<String> series = new List<string>();
        List<String> brands = new List<string>();
        #endregion

        #region Constructor
        public MovieDomainService(IRepository<MovieYear> timeGroupingRepository, IRepository<Movie> sampleRepository, IRepository<MovieStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper,
            IInflationCalculator inflationCalculator)
            : base(timeGroupingRepository, sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
        {
            this._objectMapper = objectMapper;
            this._inflationCalculator = inflationCalculator;
        }
        #endregion

        #region PopulateSamples
        public async Task PopulateSamples()
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                this.DeleteSamples();
                List<MovieYear> movieYears;

                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    movieYears = this._timeGroupingRepository.GetAllList();
                    //movieYears = this._timeGroupingRepository.GetAll().Where(x => x.Start.Year == 2015).ToList();
                    await unitOfWork.CompleteAsync();
                }

                foreach (MovieYear movieYear in movieYears.OrderBy(x => x.Start))
                {
                    await ScrapeYear(movieYear);
                }
            }

            genres.GenEnum("MachineLearningBP.Entities.Movies.Enums", "MovieGenres");
            ratings.GenEnum("MachineLearningBP.Entities.Movies.Enums", "MpaaRatings");
            series.GenEnum("MachineLearningBP.Entities.Movies.Enums", "MovieSeries");
            brands.GenEnum("MachineLearningBP.Entities.Movies.Enums", "MovieBrands");
        }
        #endregion

        #region ScrapeYear
        public async Task ScrapeYear(MovieYear movieYear)
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy, prefix: movieYear.Start.Year.ToString()))
            {
                try
                {
                    HtmlDocument doc = new HtmlDocument();
                    HtmlWeb getHtml = new HtmlWeb();

                    doc = getHtml.Load(String.Format(YearlyWideReleasesUrlFormatter, 1, movieYear.Start));

                    List<HtmlNode> links = doc.DocumentNode.QuerySelectorAll("a").ToList();
                    int totalPages = (links.Count(x => x.GetAttributeValue("href", String.Empty).StartsWith("/yearly/chart/?page=", StringComparison.CurrentCulture)) / 2) + 1;

                    for (int i = 1; i <= totalPages; i++)
                    {
                        if (i != 1)
                        {
                            doc = getHtml.Load(String.Format(YearlyWideReleasesUrlFormatter, i, movieYear.Start));
                        }

                        //Movie table
                        HtmlNode movieTable = doc.DocumentNode.QuerySelectorAll("table").First(x => x.GetAttributeValue("bgcolor", String.Empty) == "#ffffff");
                        List<HtmlNode> movieRows = movieTable.Elements("tr").ToList();
                        int movieRowsCount = movieRows.Count;

                        var tasks = movieRows.Skip(1).Take(movieRowsCount - 5).Select(x => ScrapeMovie(x, movieYear.Start.Year)).ToArray();
                        using (var unitOfWork = this.UnitOfWorkManager.Begin())
                        {
                            MovieScrapeDto[] movieDtos = await Task.WhenAll(tasks);

                            foreach (MovieScrapeDto movieDto in movieDtos)
                            {
                                if (String.IsNullOrEmpty(movieDto.Title)) continue;

                                Movie movie = this._sampleRepository.GetAllIncluding(x => x.StatLines).FirstOrDefault(x => x.MojoId == movieDto.MojoId);
                                if (movie == null)
                                {
                                    movie = movieDto.MapTo<Movie>();
                                    movie.TimeGroupingId = movieYear.Id;

                                    movie.StatLines = new List<MovieStatLine>();
                                    MovieStatLine movieStatLine = movieDto.Stats.MapTo<MovieStatLine>();
                                    movie.StatLines.Add(movieStatLine);

                                    this._sampleRepository.Insert(movie);
                                }
                                else
                                {
                                    movieDto.MapTo(movie);
                                    movieDto.Stats.MapTo(movie.StatLines.ElementAt(0));
                                }

                                //Enums
                                if (!String.IsNullOrEmpty(movieDto.Stats.GenreString) && !genres.Contains(movieDto.Stats.GenreString)) genres.Add(movieDto.Stats.GenreString);
                                if (!String.IsNullOrEmpty(movieDto.Stats.RatingString) && !ratings.Contains(movieDto.Stats.RatingString)) ratings.Add(movieDto.Stats.RatingString);
                                if (!String.IsNullOrEmpty(movieDto.Stats.BrandString) && !brands.Contains(movieDto.Stats.BrandString)) brands.Add(movieDto.Stats.BrandString);
                                if (!String.IsNullOrEmpty(movieDto.Stats.MicroSeriesString) && !series.Contains(movieDto.Stats.MicroSeriesString)) series.Add(movieDto.Stats.MicroSeriesString);
                                if (!String.IsNullOrEmpty(movieDto.Stats.MacroSeriesString) && !series.Contains(movieDto.Stats.MacroSeriesString)) series.Add(movieDto.Stats.MacroSeriesString);
                            }
                            unitOfWork.Complete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                }
            }
        }
        #endregion

        #region ScrapeMovie
        public async Task<MovieScrapeDto> ScrapeMovie(HtmlNode movieRow, int year)
        {
            MovieScrapeDto movie = new MovieScrapeDto();
            MovieStatLineScrapeDto movieStatLine = new MovieStatLineScrapeDto();
            movie.Stats = movieStatLine;

            try
            {
                DateTime inflateTo = Clock.Now.AddMonths(-1);

                List<HtmlNode> movieDataColumns = movieRow.Elements("td").Skip(1).ToList();

                if (movieDataColumns[5].Element("font").InnerText.Trim() == "N/A")
                {
                    return movie;
                }

                HtmlNode titleLink = movieDataColumns[0].QuerySelector("a");
                IReadOnlyDictionary<string, string> titleQueryString = (new Uri(String.Format("http://www.gl.com{0}", titleLink.GetAttributeValue("href", String.Empty)))).ParseQueryString();
                String title = titleLink.InnerText.Trim().Replace("nbsp;", String.Empty).Replace("�", String.Empty);
                String mojoId = titleQueryString["id"];

                movie.Title = title;
                movie.MojoId = mojoId.Replace("%EF%BF%BD", "%A0");
                movie.WideRelease = DateTime.Parse($"{ movieDataColumns[6].QuerySelector("a").InnerText.Trim()}/{year}");
                movie.Completed = movie.WideRelease < Clock.Now;

                #region Funcs
                Func<HtmlNode, String, bool> findNodeStartsWith = (n, s) =>
                {
                    return n.InnerHtml.StartsWith(s, StringComparison.CurrentCulture);
                };
                #endregion

                Task<bool>[] tabScrapingTasks = new Task<bool>[2];

                #region Main page
                tabScrapingTasks[0] = Task.Run(() =>
                {
                    HtmlDocument doc;
                    HtmlWeb getHtml = new HtmlWeb();

                    doc = getHtml.Load(String.Format(MovieUrlFormatter, mojoId));
                    List<HtmlNode> allTds = doc.DocumentNode.QuerySelectorAll("td").ToList();

                    if (allTds.Count() == 0)
                    {
                        return false;
                    }

                    HtmlNode ratingNode = allTds.FirstOrDefault(x => findNodeStartsWith(x, "MPAA Rating:"));
                    movieStatLine.RatingString = ratingNode.Element("b").InnerText.Trim();
                    movieStatLine.MpaaRating = EnumExtensions.ParseEnum<MpaaRatings>(movieStatLine.RatingString, MpaaRatings.None);

                    HtmlNode genreNode = allTds.FirstOrDefault(x => findNodeStartsWith(x, "Genre:"));
                    movieStatLine.GenreString = genreNode.Element("b").InnerText.Trim();
                    movieStatLine.Genre = EnumExtensions.ParseEnum<MovieGenres>(movieStatLine.GenreString, MovieGenres.None);

                    HtmlNode budgetNode = allTds.FirstOrDefault(x => findNodeStartsWith(x, "Production Budget:"));
                    movieStatLine.Budget = this._inflationCalculator.CalculateCpi(budgetNode.Element("b").InnerText.Trim().ParseMoney(), movie.WideRelease, inflateTo);

                    List<HtmlNode> allSBNodes = doc.DocumentNode.QuerySelectorAll("b").ToList();
                    allSBNodes.AddRange(doc.DocumentNode.QuerySelectorAll("a"));

                    List<HtmlNode> seriesNodes = allSBNodes.Where(x => findNodeStartsWith(x, "Series:")).ToList();
                    if (seriesNodes.Count == 1)
                    {
                        movieStatLine.MacroSeriesString = seriesNodes[0].InnerText.Replace("Series: ", String.Empty);
                        movieStatLine.MacroSeries = EnumExtensions.ParseEnum<MovieSeries>(movieStatLine.MacroSeriesString, MovieSeries.None);
                    }
                    else if (seriesNodes.Count > 1)
                    {
                        movieStatLine.MicroSeriesString = seriesNodes[0].InnerText.Replace("Series: ", String.Empty);
                        movieStatLine.MacroSeriesString = seriesNodes[1].InnerText.Replace("Series: ", String.Empty);

                        movieStatLine.MicroSeries = EnumExtensions.ParseEnum<MovieSeries>(movieStatLine.MicroSeriesString, MovieSeries.None);
                        movieStatLine.MacroSeries = EnumExtensions.ParseEnum<MovieSeries>(movieStatLine.MacroSeriesString, MovieSeries.None);
                    }

                    HtmlNode brandNode = allSBNodes.FirstOrDefault(x => findNodeStartsWith(x, "Brand:"));
                    if (brandNode != null)
                    {
                        movieStatLine.BrandString = brandNode.InnerText.Replace("Brand: ", String.Empty);
                        movieStatLine.Brand = EnumExtensions.ParseEnum<MovieBrands>(movieStatLine.BrandString, MovieBrands.None);
                    }

                    return true;
                });

                #endregion

                #region Opening Weekend
                tabScrapingTasks[1] = Task.Run(() =>
                {
                    HtmlDocument doc;
                    HtmlWeb getHtml = new HtmlWeb();
                    doc = getHtml.Load(String.Format(MovieWeekendUrlFormatter, mojoId));

                    List<HtmlNode> weekendRows = new List<HtmlNode>();

                    Dictionary<int, List<HtmlNode>> weekendDictionary = new Dictionary<int, List<HtmlNode>>();

                    int key = 0;

                    foreach (HtmlNode weekendTable in doc.DocumentNode.QuerySelectorAll(".chart-wide"))
                    {
                        if (weekendTable.PreviousSibling.Name == "b")
                        {
                            key = Int32.Parse(weekendTable.PreviousSibling.Element("font").ScrapifyNode());
                            weekendDictionary.Add(key, weekendTable.Elements("tr").Skip(1).ToList());
                        }
                        else
                        {
                            weekendDictionary[key].AddRange(weekendTable.Elements("tr").Skip(1).ToList());
                        }
                    }

                    if (weekendDictionary.Count() == 0)
                    {
                        return false;
                    }

                    Func<HtmlNode, int, bool> findOpeningWeekendRow = (node, y) =>
                    {
                        List<HtmlNode> columns = node.Elements("td").ToList();
                        HtmlNode boldNode = columns[0].QuerySelector("b");
                        String weekendString = boldNode.Elements("i").Count() > 0 ? boldNode.Element("i").InnerText.Trim() : boldNode.InnerText.Trim();
                        String[] weekendSplit = weekendString.Split(new string[] { "&#150;" }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime weekendStartDate = DateTime.Parse($"{weekendSplit[0]} {y}");
                        DateTime adjustedWideRelease = movie.WideRelease.AddDays((int)DayOfWeek.Friday - (int)movie.WideRelease.DayOfWeek);

                        return weekendStartDate == adjustedWideRelease;
                    };

                    HtmlNode openingWeekendRow = null;

                    foreach (KeyValuePair<int, List<HtmlNode>> pair in weekendDictionary)
                    {
                        openingWeekendRow = pair.Value.FirstOrDefault(x => findOpeningWeekendRow(x, pair.Key));
                        if (openingWeekendRow != null) break;
                    }

                    List<HtmlNode> openingWeekendColumns = openingWeekendRow.Elements("td").ToList();

                    if (openingWeekendRow != null)
                    {
                        movieStatLine.Opening = this._inflationCalculator.CalculateCpi(Double.Parse(openingWeekendColumns[2].Element("font").ScrapifyNode(), System.Globalization.NumberStyles.Currency), movie.WideRelease, inflateTo);
                        movieStatLine.TheaterCount = this._inflationCalculator.CalculateTheaterCount(Double.Parse(openingWeekendColumns[4].Element("font").ScrapifyNode(), System.Globalization.NumberStyles.Number), movie.WideRelease, inflateTo);
                        //movieStatLine.TheaterCount = Double.Parse(openingWeekendColumns[4].Element("font").ScrapifyNode(), System.Globalization.NumberStyles.Number);
                        movieStatLine.TheaterAverage = movieStatLine.Opening / movieStatLine.TheaterCount;

                        List<HtmlNode> allTds = doc.DocumentNode.QuerySelectorAll("td").ToList();

                        try
                        {
                            HtmlNode runtimeNode = allTds.FirstOrDefault(x => findNodeStartsWith(x, "Runtime:"));
                            List<Double> runtimeSplit = runtimeNode.Element("b").InnerText.Trim().Replace(" hrs.", String.Empty).Replace(" min.", String.Empty).Split(" ".ToCharArray()).Select(x => Double.Parse(x)).ToList();
                            movieStatLine.Runtime = (runtimeSplit[0] * 60) + runtimeSplit[1];
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else
                    {
                        this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Bad weekend: {movie.Title}, {movie.WideRelease.ToShortDateString()}"));
                    }

                    return true;
                });
                #endregion

                bool[] tabScrapingResults = await Task.WhenAll(tabScrapingTasks);

                if (tabScrapingResults.Any(x => !x))
                {
                    movie.Title = String.Empty;
                    return movie;
                }

                //movie = await this._sampleRepository.FirstOrDefaultAsync(x => x.MojoId == mojoId);
                //bool insert = movie == null;

                //if (movie == null)
                //{
                //    movie = new Movie { Title = title, MojoId = mojoId, TimeGroupingId = movieYear.Id };
                //    movieStatLine = new MovieStatLine();


                //    movie.StatLines = new List<MovieStatLine>() { movieStatLine };
                //}
                //else
                //{
                //    movieStatLine = this._statLineRepository.FirstOrDefault(x => x.SampleId == movie.Id);
                //}
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }

            return movie;
        }
        #endregion

        #region DeleteSamples
        public void DeleteSamples()
        {
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting movies..."));

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute("DELETE FROM [Movies]");
                unitOfWork.Complete();
            }

            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting movies finished."));
        } 
        #endregion
    }
}
