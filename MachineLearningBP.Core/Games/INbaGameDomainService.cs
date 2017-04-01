using HtmlAgilityPack;
using MachineLearningBP.Seasons;
using MachineLearningBP.StatLines;
using System;
using System.Threading.Tasks;

namespace MachineLearningBP.Games
{
    public interface INbaGameDomainService : IGameDomainService
    {
        Task ScrapeGamesForDate(DateTime currentDate, DateTime now, NbaSeason season);
        Task ScrapeGame(HtmlNode matchupBox, DateTime currentDate, DateTime now, NbaSeason season);
        void ScrapeStatTable(NbaStatLine statLine, HtmlNode summaryRow);
    }
}
