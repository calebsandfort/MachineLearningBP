using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MachineLearningBP.Entities.Sports.Nba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace MachineLearningBP.Tests.Sports.Nba
{
    public class Nba_Tests : MachineLearningBPTestBase
    {
        private readonly IRepository<NbaTeam> _nbaTeamRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public Nba_Tests()
        {
            _nbaTeamRepository = Resolve<IRepository<NbaTeam>>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
        }

        //[Fact]
        public async Task NbaTeamCount_Test()
        {
            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                int teamCount = await _nbaTeamRepository.CountAsync();
                teamCount.ShouldBe(30);

                uow.Complete();
            }
        }
    }
}
