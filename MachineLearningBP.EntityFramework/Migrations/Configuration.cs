using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using MachineLearningBP.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace MachineLearningBP.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<MachineLearningBP.EntityFramework.MachineLearningBPDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MachineLearningBP";
        }

        protected override void Seed(MachineLearningBP.EntityFramework.MachineLearningBPDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();

                //new MlbSeasonsCreator(context).Create();
                //new MlbTeamsCreator(context).Create();
                //new NbaSeasonsCreator(context).Create();
                //new NbaTeamsCreator(context).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
