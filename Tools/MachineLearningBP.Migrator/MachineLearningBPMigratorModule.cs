using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using MachineLearningBP.EntityFramework;

namespace MachineLearningBP.Migrator
{
    [DependsOn(typeof(MachineLearningBPDataModule))]
    public class MachineLearningBPMigratorModule : AbpModule
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