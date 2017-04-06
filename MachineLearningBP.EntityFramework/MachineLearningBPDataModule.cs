using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using MachineLearningBP.EntityFramework;
using MachineLearningBP.Shared;

namespace MachineLearningBP
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(MachineLearningBPCoreModule), typeof(MachineLearningBPCollectiveIntelligenceModule))]
    public class MachineLearningBPDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<MachineLearningBPDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
