using System.Web.Mvc;
using MachineLearningBP.Services.Sports.Nba;
using MachineLearningBP.Web.Framework;
using Abp.Domain.Repositories;
using MachineLearningBP.Entities.Sports.Nba;
using Kendo.Mvc.UI;
using System.Linq;
using MachineLearningBP.Core.Entities.Sports.Nba.Dtos;
using System.Collections.Generic;
using Abp.ObjectMapping;
using System;
using Abp.Timing;
using System.Linq.Expressions;

namespace MachineLearningBP.Web.Controllers
{
    //[AbpMvcAuthorize]
    public class NbaController : MachineLearningBPControllerBase
    {
        readonly INbaPointsExampleAppService _nbaPointsExampleAppService;
        readonly IRepository<NbaGame> _gameRepository;
        readonly IObjectMapper _objectMapper;

        public NbaController(INbaPointsExampleAppService nbaPointsExampleAppService, IRepository<NbaGame> gameRepository, IObjectMapper objectMapper)
        {
            _nbaPointsExampleAppService = nbaPointsExampleAppService;
            _gameRepository = gameRepository;
            _objectMapper = objectMapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Today_Read
        public ActionResult Today_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = new DataSourceResult();

            DateTime currentDate = Clock.Now;

            Expression<Func<NbaGame, bool>> todayFunc = x => x.Date.Year == currentDate.Year && x.Date.Month == currentDate.Month && x.Date.Day == currentDate.Day;

            result.Data = _objectMapper.Map<List<NbaGameDto>>(_gameRepository.GetAllIncluding(x => x.StatLines.Select(y => y.Participant)).Where(request.Filters).Where(todayFunc).OrderBy(request.Sorts[0]).ToList());
            result.Total = _gameRepository.GetAll().Where(request.Filters).Where(todayFunc).Count();

            return new GuerillaLogisticsApiJsonResult(result);
        }
        #endregion

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