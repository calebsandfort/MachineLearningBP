using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MachineLearningBP.Entities.Movies;
using MachineLearningBP.Shared.InflationCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MachineLearningBP.Tests.InflationCalculator
{
    public class InflationCalculator_Tests : MachineLearningBPTestBase
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IInflationCalculator _inflationCalculator;

        public InflationCalculator_Tests()
        {
            _inflationCalculator = Resolve<IInflationCalculator>();
            _movieRepository = Resolve<IRepository<Movie>>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
        }

        [Fact(DisplayName = "InflationCalculator.GenCpiDictionary")]
        public void GenCpiDictionary_Test()
        {
            this._inflationCalculator.GenCpiDictionary();
        }

        [Fact(DisplayName = "Movies.GenTheaterCountDictionary")]
        public void GenTheaterCountDictionary_Test()
        {
            List<KeyValuePair<int, double>> theaterCounts;

            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                theaterCounts = (from m in _movieRepository.GetAllIncluding(x => x.StatLines)
                                 group m by m.WideRelease.Year into g
                                 select g).ToList().Select(x => new KeyValuePair<int, Double>(x.Key, x.OrderByDescending(y => y.StatLines.First().TheaterCount).Take(5).Average(z => z.StatLines.First().TheaterCount))).ToList();

                uow.Complete();
            }

            this._inflationCalculator.GenTheaterCountDictionary(theaterCounts);
        }
    }
}
