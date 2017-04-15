using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using MachineLearningBP.Services.Sports.Nba;
using System.Threading.Tasks;
using MachineLearningBP.CollectiveIntelligence.Entities;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CsvHelper;

namespace MachineLearningBP.Web.Controllers
{
    //[AbpMvcAuthorize]
    public class NbaController : MachineLearningBPControllerBase
    {
        private readonly INbaPointsExampleAppService _nbaPointsExampleAppService;

        public NbaController(INbaPointsExampleAppService nbaPointsExampleAppService)
        {
            _nbaPointsExampleAppService = nbaPointsExampleAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        //public async Task<FileContentResult> GetPointsKnnBestParametersCsv()
        //{
        //    List<KNearestNeighborsCrossValidateResult> results = await this._nbaPointsExampleAppService.FindOptimalParameters();

        //    using (var stream = new MemoryStream())
        //    using (var writer = new StreamWriter(stream))
        //    using (var csv = new CsvWriter(writer))
        //    {
        //        //csv.Configuration.RegisterClassMap<GameExampleIncludeGamblingMap>();
        //        csv.WriteRecords(results);
        //        writer.Flush();
        //        stream.Position = 0;

        //        return File(new UTF8Encoding().GetBytes((new StreamReader(stream)).ReadToEnd().TrimEnd('\r', '\n')), "text/csv", "PointsKnnBestParameters.csv");
        //    }
        //}
    }
}