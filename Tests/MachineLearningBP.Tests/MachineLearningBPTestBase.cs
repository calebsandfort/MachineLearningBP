using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.TestBase;
using MachineLearningBP.EntityFramework;
using MachineLearningBP.Migrations.SeedData;
using MachineLearningBP.MultiTenancy;
using MachineLearningBP.Users;
using Castle.MicroKernel.Registration;
using Effort;
using EntityFramework.DynamicFilters;
using System.Configuration;
using System.Data.SqlClient;

namespace MachineLearningBP.Tests
{
    public abstract class MachineLearningBPTestBase : AbpIntegratedTestBase<MachineLearningBPTestModule>
    {
        private readonly string _connectionString;

        protected MachineLearningBPTestBase()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
            _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            Resolve<IAbpStartupConfiguration>().DefaultNameOrConnectionString = _connectionString;
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            //LocalIocManager.IocContainer.Register(
            //    Component.For<DbConnection>()
            //             .UsingFactoryMethod(() =>
            //             {
            //                 var connection = new SqlConnection(_connectionString);
            //                 return connection;
            //             })
            //             .LifestyleSingleton()
            //);
        }
    }
}