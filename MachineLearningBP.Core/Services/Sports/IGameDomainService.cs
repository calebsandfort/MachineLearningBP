using HtmlAgilityPack;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Samples;
using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Entities.Sports.Nfl.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MachineLearningBP.Core.Services.Sports
{
    public interface IGameDomainService<TTimeGrouping, TStatLine> : ISampleDomainService
        where TTimeGrouping : TimeGrouping
        where TStatLine : StatLine
    {
        Task FillSamples();
        Task ScrapeGamesForDate(DateTime currentDate, DateTime now, TTimeGrouping season, List<NflPlay> plays = null);
        Task ScrapeGame(HtmlNode matchupBox, DateTime currentDate, DateTime now, TTimeGrouping season, List<NflPlay> plays = null);
    }
}
