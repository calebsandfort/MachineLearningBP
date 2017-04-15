using System;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;
using System.Web.Mvc;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos.Enums;

namespace MachineLearningBP.Web
{
    public class MvcApplication : AbpWebApplication<MachineLearningBPWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config")
            );

            ModelBinders.Binders.Add(typeof(GeneticOptimizeTargets), new EnumModelBinder<GeneticOptimizeTargets>());

            base.Application_Start(sender, e);
        }
    }
}
