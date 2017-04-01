using System.Data.Common;
using Abp.Zero.EntityFramework;
using MachineLearningBP.Authorization.Roles;
using MachineLearningBP.MultiTenancy;
using MachineLearningBP.Users;
using System.Data.Entity;
using MachineLearningBP.Seasons;
using MachineLearningBP.Games;
using MachineLearningBP.Teams;
using MachineLearningBP.StatLines;

namespace MachineLearningBP.EntityFramework
{
    public class MachineLearningBPDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        public virtual IDbSet<NbaSeason> NbaSeasons { get; set; }
        public virtual IDbSet<NbaGame> NbaGames { get; set; }
        public virtual IDbSet<NbaTeam> NbaTeams { get; set; }
        public virtual IDbSet<NbaStatLine> NbaStatLines { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public MachineLearningBPDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in MachineLearningBPDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of MachineLearningBPDbContext since ABP automatically handles it.
         */
        public MachineLearningBPDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public MachineLearningBPDbContext(DbConnection existingConnection)
         : base(existingConnection, false)
        {

        }

        public MachineLearningBPDbContext(DbConnection existingConnection, bool contextOwnsConnection)
         : base(existingConnection, contextOwnsConnection)
        {

        }
    }
}
