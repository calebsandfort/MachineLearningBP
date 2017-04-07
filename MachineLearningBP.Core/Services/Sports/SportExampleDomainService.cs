using MachineLearningBP.CollectiveIntelligence.DomainServices;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Domain.Repositories;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.Shared.GuerillaTimer;
using MachineLearningBP.Entities.Sports;
using MachineLearningBP.Shared.Dtos;

namespace MachineLearningBP.Services.Sports
{
    public abstract class SportExampleDomainService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo, TTimeGrouping, TParticipant>
        : MaximumExampleDomainService<TSample, TStatLine, TExample, TResult, TExampleGenerationInfo, TTimeGrouping, TParticipant>,
        ISportExampleDomainService<TSample, TParticipant>
        where TSample : Sample
        where TStatLine : StatLine
        where TTimeGrouping : TimeGrouping
        where TExampleGenerationInfo : ExampleGenerationInfo
        where TExample : Example<TResult, TExampleGenerationInfo>
        where TParticipant : Participant
    {
        public SportExampleDomainService(IRepository<TSample> sampleRepository, IRepository<TStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager,
            IRepository<TExample> exampleRepository, IRepository<TTimeGrouping> timeGroupingRepository,
            IRepository<TParticipant> participantRepository)
            : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, exampleRepository, timeGroupingRepository, participantRepository)
        {
        }
        #region PopulateExamples
        public abstract Task PopulateExample(TSample game, List<TSample> games, List<TParticipant> teams, int rollingWindowPeriod, Double scaleFactor);
        //public abstract async Task PopulateExamples(int rollingWindowPeriod, Double scaleFactor)
        //{
        //    using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
        //    {
        //        this.DeleteExamples();

        //        DateTime currentDate = this._sampleRepository.GetAll().Where(x => x.Completed).OrderBy(x => x.Date).First().Date;
        //        List<TTimeGrouping> seasons;
        //        List<TParticipant> teams;
        //        List<TSample> games, todaysGames;

        //        seasons = this._timeGroupingRepository.GetAll().OrderBy(x => x.Start).ToList();
        //        teams = await this._participantRepository.GetAllListAsync();


        //        foreach (TTimeGrouping season in seasons.OrderBy(x => x.Start))
        //        {
        //            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Starting {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season..."));

        //            games = this._sampleRepository.GetAllIncluding(x => x.StatLines)
        //                    .Where(x => x.TimeGroupingId == season.Id).ToList();

        //            if (currentDate < season.Start.Date) currentDate = season.RollingWindowStart.Value.Date;

        //            while (season.Within(currentDate))
        //            {
        //                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Populating {currentDate.ToShortDateString()} ..."));

        //                todaysGames = this._sampleRepository.GetAll().Where(x => x.Date.Year == currentDate.Year && x.Date.Month == currentDate.Month && x.Date.Day == currentDate.Day).ToList();

        //                await Task.WhenAll(todaysGames.Select(x => PopulateExample(x, games, teams, rollingWindowPeriod, scaleFactor)).ToArray());

        //                currentDate = currentDate.AddDays(1);
        //            }

        //            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Completed {season.Start.ToShortDateString()} - {season.End.ToShortDateString()} season."));
        //        }
        //    }
        //}
        #endregion

        #region PopulateExample
        public abstract Task PopulateExamples(int rollingWindowPeriod, double scaleFactor);

        //public async Task PopulateExample(TSample game, List<TSample> games, List<TParticipant> teams, int rollingWindowPeriod, Double scaleFactor)
        //{
        //    using (var unitOfWork = this.UnitOfWorkManager.Begin())
        //    {
        //        TExampleGenerationInfo awayInfo = (TExampleGenerationInfo)Activator.CreateInstance(typeof(TExampleGenerationInfo), game, games, teams, false, rollingWindowPeriod, scaleFactor);
        //        TExample awayExample = Activator.CreateInstance<TExample>();
        //        awayExample.SetFields(awayInfo);
        //        this._exampleRepository.Insert(awayExample);

        //        TExampleGenerationInfo homeInfo = (TExampleGenerationInfo)Activator.CreateInstance(typeof(TExampleGenerationInfo), game, games, teams, false, rollingWindowPeriod, scaleFactor);
        //        TExample homeExample = Activator.CreateInstance<TExample>();
        //        homeExample.SetFields(homeInfo);
        //        this._exampleRepository.Insert(homeExample);

        //        await unitOfWork.CompleteAsync();
        //    }
        //}
        #endregion

        #region DeleteExamples
        public abstract void DeleteExamples();
        //public abstract void DeleteExamples(String table)
        //{
        //    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Deleting {table}..."));

        //    using (var unitOfWork = this.UnitOfWorkManager.Begin())
        //    {
        //        this._sqlExecuter.Execute($"DELETE FROM [{table}]");
        //        unitOfWork.Complete();
        //    }

        //    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Deleting {table} finished."));
        //}
        #endregion
    }
}
