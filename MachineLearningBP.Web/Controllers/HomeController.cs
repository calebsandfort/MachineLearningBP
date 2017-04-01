using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace MachineLearningBP.Web.Controllers
{
    //[AbpMvcAuthorize]
    public class HomeController : MachineLearningBPControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}