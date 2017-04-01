using Abp.Domain.Services;
using MachineLearningBP.Seasons;
using System;
using System.Threading.Tasks;

namespace MachineLearningBP.Seasons
{
    public interface ISeasonDomainService : IDomainService
    {
        Task SetSeasonsRollingWindowStart();
    }
}
