using Abp.Domain.Services;
using MachineLearningBP.Entities.Sports.Nba;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MachineLearningBP.Core.Services.Sports.Nba
{
    public interface INbaSeasonDomainService : IDomainService
    {
        bool SetSeasonRollingWindowStart(NbaSeason season, List<NbaTeam> teams, DateTime currentDate);
        Task SetSeasonsRollingWindowStart();
    }
}
