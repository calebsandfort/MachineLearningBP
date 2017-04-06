using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Framework
{
    public class MySettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                {
                    new SettingDefinition(
                        "Path",
                        "http://dev-csandfort.gwi.com/MachineLearningBP.Web/",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "NbaRollingWindowPeriod",
                        "10",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "MlbRollingWindowPeriod",
                        "20",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "NbaScaleFactor",
                        "100",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "MlbScaleFactor",
                        "100",
                        scopes: SettingScopes.Application
                        )
                };
        }
    }
}
