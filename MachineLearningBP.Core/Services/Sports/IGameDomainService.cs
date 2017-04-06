using Abp.Domain.Services;
using HtmlAgilityPack;
using MachineLearningBP.CollectiveIntelligence.DomainServices;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Threading.Tasks;

namespace MachineLearningBP.Core.Services.Sports
{
    public interface IGameDomainService<TTimeGrouping, TStatLine> : ISampleDomainService
        where TTimeGrouping : TimeGrouping
        where TStatLine : StatLine
    {
        Task FillSamples();
        Task ScrapeGamesForDate(DateTime currentDate, DateTime now, TTimeGrouping season);
        Task ScrapeGame(HtmlNode matchupBox, DateTime currentDate, DateTime now, TTimeGrouping season);
        void ScrapeStatTable(TStatLine statLine, HtmlNode summaryRow);
    }
}
