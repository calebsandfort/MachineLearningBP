using Abp.Dependency;
using Abp.EntityFramework;
using MachineLearningBP.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Framework
{
    public class SqlExecuter : ISqlExecuter, ITransientDependency
    {
        private readonly IDbContextProvider<MachineLearningBPDbContext> _dbContextProvider;

        public SqlExecuter(IDbContextProvider<MachineLearningBPDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public int Execute(string sql, params object[] parameters)
        {
            return _dbContextProvider.GetDbContext().Database.ExecuteSqlCommand(sql, parameters);
        }
    }
}
