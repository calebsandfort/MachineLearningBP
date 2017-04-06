using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using MachineLearningBP.Shared;

namespace MachineLearningBP
{
    [DependsOn(typeof(MachineLearningBPCoreModule), typeof(AbpAutoMapperModule), typeof(MachineLearningBPCollectiveIntelligenceModule))]
    public class MachineLearningBPApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                //Add your custom AutoMapper mappings here...
                //mapper.CreateMap<,>()
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
