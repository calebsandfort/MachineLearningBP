using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos.Enums;
using MachineLearningBP.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MachineLearningBP.Web.Controllers
{
    public class ViewRendererController : MachineLearningBPControllerBase
    {
        // GET: ViewRenderer
        [HttpPost]
        public ActionResult GeneticOptimizeModal(GeneticOptimizeTargets target)
        {
            return PartialView("Modals/_GeneticOptimizeModal", target);
        }

        public ActionResult GetKNearestNeighborsGuessMethods()
        {
            return new GuerillaLogisticsApiJsonResult(Extensions.EnumToListItems<KNearestNeighborsGuessMethods>());
        }

        public ActionResult GetKNearestNeighborsWeightMethods()
        {
            return new GuerillaLogisticsApiJsonResult(Extensions.EnumToListItems<KNearestNeighborsWeightMethods>());
        }
    }
}