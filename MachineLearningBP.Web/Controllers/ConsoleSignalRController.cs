using System.Web.Mvc;
using MachineLearningBP.Web.Hubs;
using MachineLearningBP.Shared.Dtos;

namespace MachineLearningBP.Web.Controllers
{
    //[AbpMvcAuthorize]
    public class ConsoleSignalRController : MachineLearningBPControllerBase
    {
        private readonly ConsoleHub _consoleHub;

        public ConsoleSignalRController(ConsoleHub consoleHub)
        {
            _consoleHub = consoleHub;
        }

        [HttpPost]
        public ActionResult WriteLine(ConsoleWriteLineInput input)
        {
            this._consoleHub.WriteLine(input);
            return this.Json(new { success = true });
        }
    }
}