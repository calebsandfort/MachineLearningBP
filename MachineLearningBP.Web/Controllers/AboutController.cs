using System.Web.Mvc;

namespace MachineLearningBP.Web.Controllers
{
    public class AboutController : MachineLearningBPControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}