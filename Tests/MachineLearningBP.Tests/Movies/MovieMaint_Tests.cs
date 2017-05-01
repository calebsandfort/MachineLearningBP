using MachineLearningBP.Core.Services.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace MachineLearningBP.Tests.Movies
{
    public class MovieMaint_Tests : MachineLearningBPTestBase
    {
        private readonly IMovieDomainService _movieDomainService;

        public MovieMaint_Tests()
        {
            _movieDomainService = Resolve<IMovieDomainService>();
        }

        [Fact(DisplayName = "Movies.PopulateSamples")]
        public async Task PopulateSamples_Test()
        {
            await this._movieDomainService.PopulateSamples();
        }
    }
}
