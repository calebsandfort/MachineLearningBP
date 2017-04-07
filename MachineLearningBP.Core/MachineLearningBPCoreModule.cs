using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using MachineLearningBP.Authorization;
using MachineLearningBP.Authorization.Roles;
using MachineLearningBP.MultiTenancy;
using MachineLearningBP.Users;
using MachineLearningBP.Framework;
using MachineLearningBP.Shared;
using MachineLearningBP.Services.Sports;
using Abp.Dependency;

namespace MachineLearningBP
{
    [DependsOn(typeof(AbpZeroCoreModule), typeof(MachineLearningBPSharedModule), typeof(MachineLearningBPCollectiveIntelligenceModule))]
    public class MachineLearningBPCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            //Remove the following line to disable multi-tenancy.
            Configuration.MultiTenancy.IsEnabled = MachineLearningBPConsts.MultiTenancyEnabled;

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    MachineLearningBPConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "MachineLearningBP.Localization.Source"
                        )
                    )
                );

            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Authorization.Providers.Add<MachineLearningBPAuthorizationProvider>();

            Configuration.Settings.Providers.Add<MySettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
