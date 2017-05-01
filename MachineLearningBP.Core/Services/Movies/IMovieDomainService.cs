using HtmlAgilityPack;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Samples;
using MachineLearningBP.Entities.Movies;
using System.Threading.Tasks;

namespace MachineLearningBP.Core.Services.Movies
{
    public interface IMovieDomainService : ISampleDomainService
    {
        Task ScrapeYear(MovieYear movieYear);
        Task<MovieScrapeDto> ScrapeMovie(HtmlNode movieRow, int year);
    }
}
