using MachineLearningBP.Teams;
using System;
using System.Collections.Generic;

namespace MachineLearningBP.Seasons
{
    public interface INbaSeasonDomainService : ISeasonDomainService
    {
        bool SetSeasonRollingWindowStart(NbaSeason season, List<NbaTeam> teams, DateTime currentDate);
    }
}
