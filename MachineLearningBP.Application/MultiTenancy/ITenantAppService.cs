using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MachineLearningBP.MultiTenancy.Dto;

namespace MachineLearningBP.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultDto<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
