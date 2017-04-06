using Abp.Domain.Services;
using MachineLearningBP.Entities.Sports.Mlb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MachineLearningBP.Core.Services.Sports.Mlb
{
    public interface IMlbSeasonDomainService : IDomainService
    {
        bool SetSeasonRollingWindowStart(MlbSeason season, List<MlbTeam> teams, DateTime currentDate);
        Task SetSeasonsRollingWindowStart();
    }
}
