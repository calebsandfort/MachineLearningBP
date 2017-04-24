using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.MultiTenancy;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using MachineLearningBP.Shared;
using NSubstitute;
using System.Reflection;

namespace MachineLearningBP.Tests
{
    [DependsOn(typeof(MachineLearningBPApplicationModule), typeof(MachineLearningBPSharedModule), typeof(MachineLearningBPCollectiveIntelligenceModule),
        typeof(MachineLearningBPCoreModule), typeof(MachineLearningBPDataModule), typeof(AbpTestBaseModule))]
    public class MachineLearningBPTestModule : AbpModule
    {
        private readonly string _connectionString;

        public MachineLearningBPTestModule()
        {
            
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
