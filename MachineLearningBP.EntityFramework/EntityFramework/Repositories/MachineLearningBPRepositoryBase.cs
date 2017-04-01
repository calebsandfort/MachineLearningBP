using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace MachineLearningBP.EntityFramework.Repositories
{
    public abstract class MachineLearningBPRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<MachineLearningBPDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected MachineLearningBPRepositoryBase(IDbContextProvider<MachineLearningBPDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class MachineLearningBPRepositoryBase<TEntity> : MachineLearningBPRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected MachineLearningBPRepositoryBase(IDbContextProvider<MachineLearningBPDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
