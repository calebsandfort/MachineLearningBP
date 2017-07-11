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
                        "http://localhost/MachineLearningBP.Web/",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "NbaRollingWindowPeriod",
                        "10",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "NflRollingWindowPeriod",
                        "3",
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
                        ),
                    new SettingDefinition(
                        "GoogleAccessToken",
                        "",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "GoogleRefreshToken",
                        "",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "DriveFolder.NbaPointsExampleDomainService.FindOptimalParameters",
                        "0B-lqH9hRWMWCam50TGhFdFEyb0U",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "DriveFolder.NbaPointsExampleDomainService.FindOptimalParametersPython",
                        "0B-lqH9hRWMWCam50TGhFdFEyb0U",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "DriveFolder.NbaPointsExampleDomainService.GeneticOptimize",
                        "0B-lqH9hRWMWCOWNqV3picUp1Qjg",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "DriveFolder.MovieOpeningWeekendExampleDomainService.FindOptimalParametersPythonAndR",
                        "0B-lqH9hRWMWCUHltbEpvcmpRb0k",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "DriveFolder.NflPointsExampleDomainService.FindOptimalParametersPython",
                        "0B-lqH9hRWMWCam50TGhFdFEyb0U",
                        scopes: SettingScopes.Application
                        ),
                    new SettingDefinition(
                        "DriveFolder.NflPointsExampleDomainService.GeneticOptimize",
                        "0B-lqH9hRWMWCOWNqV3picUp1Qjg",
                        scopes: SettingScopes.Application
                        )
                };
        }
    }
}
