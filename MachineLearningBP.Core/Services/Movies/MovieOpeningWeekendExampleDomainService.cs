using MachineLearningBP.CollectiveIntelligence.DomainServices.Examples;
using MachineLearningBP.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Repositories;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms;
using MachineLearningBP.Shared.CommandRunner;
using MachineLearningBP.Core.Services;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Shared.Dtos;
using Abp.Timing;
using MachineLearningBP.Shared.GuerillaTimer;

namespace MachineLearningBP.Services.Movies
{
    public class MovieOpeningWeekendExampleDomainService : MaximumExampleDomainService<Movie, MovieStatLine, MovieOpeningWeekendExample, Double, MovieExampleGenerationInfo, MovieYear, MovieParticipant>, IMovieOpeningWeekendExampleDomainService
    {
        #region Properties
        public readonly IKNearestNeighborsDomainService<MovieOpeningWeekendExample, MovieStatLine, Double> _kNearestNeighborsDomainService;
        private readonly ISheetUtilityDomainService _sheetUtilityDomainService;
        public readonly ICommandRunner _commandRunner;
        #endregion

        #region Ctor
        public MovieOpeningWeekendExampleDomainService(IRepository<Movie> sampleRepository, IRepository<MovieStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager,
            IRepository<MovieOpeningWeekendExample> exampleRepository, IRepository<MovieYear> timeGroupingRepository,
            IRepository<MovieParticipant> participantRepository, IBackgroundJobManager backgroundJobManager,
            IKNearestNeighborsDomainService<MovieOpeningWeekendExample, MovieStatLine, Double> kNearestNeighborsDomainService,
            ICommandRunner commandRunner, ISheetUtilityDomainService sheetUtilityDomainService)
            : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, exampleRepository, timeGroupingRepository, participantRepository, backgroundJobManager)
        {
            this._kNearestNeighborsDomainService = kNearestNeighborsDomainService;
            this._sheetUtilityDomainService = sheetUtilityDomainService;
            this._commandRunner = commandRunner;
        }
        #endregion

        #region PopulateExamples
        public async Task PopulateExamples()
        {
            await PopulateExamples(0, 0);
        }

        public async Task PopulateExamples(int rollingWindowPeriod, double scaleFactor)
        {
            try
            {
                using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
                {
                    this.DeleteExamples();

                    DateTime now = DateTime.Now.Date;
                    List<MovieYear> years;
                    List<Movie> movies;

                    using (var unitOfWork = this.UnitOfWorkManager.Begin())
                    {
                        years = this._timeGroupingRepository.GetAll().OrderBy(x => x.Start).ToList();
                        await unitOfWork.CompleteAsync();
                    }

                    foreach (MovieYear year in years.OrderBy(x => x.Start))
                    {
                        this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Starting {year.Start.ToShortDateString()} - {year.End.ToShortDateString()} year..."));

                        using (var unitOfWork = this.UnitOfWorkManager.Begin())
                        {
                            movies = this._sampleRepository.GetAllIncluding(x => x.StatLines)
                                .Where(x => x.TimeGroupingId == year.Id && x.WideRelease < now).ToList();
                            await unitOfWork.CompleteAsync();
                        }

                        await Task.WhenAll(movies.Select(x => PopulateExample(x)).ToArray());

                        this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Completed {year.Start.ToShortDateString()} - {year.End.ToShortDateString()} year."));
                    }
                }
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
                throw ex;
            }
        }
        #endregion

        #region PopulateExample
        public async Task PopulateExample(Movie movie)
        {
            try
            {
                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    MovieExampleGenerationInfo info = new MovieExampleGenerationInfo(movie);
                    MovieOpeningWeekendExample example = new MovieOpeningWeekendExample();
                    example.SetFields(info.StatLine, info);
                    await this._exampleRepository.InsertAsync(example);

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

        #region FindOptimalParametersPythonAndR
        public async Task<List<KNearestNeighborsCrossValidateResult>> FindOptimalParametersPythonAndR(bool record)
        {
            MovieOpeningWeekendExample[] data = await this.GetExamples();
            
            List<KNearestNeighborsCrossValidateResult> results = this._kNearestNeighborsDomainService.FindOptimalParametersPythonAndR(data);

            if (record && results.Count > 0)
                await this._sheetUtilityDomainService.Record(new List<IRecordContainer>(results));

            return results;
        }
        #endregion

        #region GetExamples
        public async Task<MovieOpeningWeekendExample[]> GetExamples()
        {
            MovieOpeningWeekendExample[] data;

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                //data = this._exampleRepository.GetAll().Where(x => x.WideRelease < Clock.Now.Date && x.StatLine.TheaterCount > 1000).ToArray();
                data = this._exampleRepository.GetAll().Where(x => x.WideRelease < Clock.Now.Date && x.StatLine.TheaterCount > 1000 && x.StatLine.Sample.TimeGrouping.Start.Year == 2016).OrderByDescending(x => x.StatLine.Sample.WideRelease).ToArray();
                unitOfWork.Complete();
            }

            return data;
        }
        #endregion

        #region DeleteExamples
        public void DeleteExamples()
        {
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Deleting MovieOpeningWeekendExamples..."));

            using (var unitOfWork = this.UnitOfWorkManager.Begin())
            {
                this._sqlExecuter.Execute($"DELETE FROM [MovieOpeningWeekendExamples]");
                unitOfWork.Complete();
            }

            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Deleting MovieOpeningWeekendExamples finished."));
        } 
        #endregion
    }
}
