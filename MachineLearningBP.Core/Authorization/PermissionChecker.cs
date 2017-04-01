using Abp.Authorization;
using MachineLearningBP.Authorization.Roles;
using MachineLearningBP.MultiTenancy;
using MachineLearningBP.Users;

namespace MachineLearningBP.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
