using System.Threading.Tasks;
using Abp.Application.Services;
using MachineLearningBP.Roles.Dto;

namespace MachineLearningBP.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
