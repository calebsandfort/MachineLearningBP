using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using MachineLearningBP.Authorization;
using MachineLearningBP.MultiTenancy;

namespace MachineLearningBP.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController : MachineLearningBPControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public ActionResult Index()
        {
            var output = _tenantAppService.GetTenants();
            return View(output);
        }
    }
}