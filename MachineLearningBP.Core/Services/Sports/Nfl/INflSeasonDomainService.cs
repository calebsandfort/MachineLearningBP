using Abp.Domain.Services;
using MachineLearningBP.Entities.Sports.Nfl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MachineLearningBP.Core.Services.Sports.Nfl
{
    public interface INflSeasonDomainService : IDomainService
    {
        bool SetSeasonRollingWindowStart(NflSeason season, List<NflTeam> teams, DateTime currentDate);
        Task SetSeasonsRollingWindowStart();
    }
}
