using Abp.Modules;
using MachineLearningBP.EntityFramework;
using MachineLearningBP.Shared;
using System.Data.Entity;
using System.Reflection;

namespace MachineLearningBP.ConsoleApp
{
    [DependsOn(typeof(MachineLearningBPApplicationModule), typeof(MachineLearningBPSharedModule), typeof(MachineLearningBPCollectiveIntelligenceModule)
        , typeof(MachineLearningBPCoreModule), typeof(MachineLearningBPDataModule))]
    public class MachineLearningBPConsoleAppModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<MachineLearningBPDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
