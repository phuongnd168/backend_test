using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.Data.SQL.Contexts;
using TLS.Core.Data;
using TLS.Lib.Data.SQL;

namespace TLS.BHL.Infra.Data.SQL
{
    public class BHLRepositoryBase<T> : RepositoryBase<T> where T : class, IRepository
    {
        protected BHLRepositoryBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        protected override string DefaultConnectionName
        {
            get
            {
                return "BHL";
            }
        }

        protected void UseDefaultDbContext(Action<BHLSqlDbContext> action)
        {
            UseDbContext(action);
        }
        protected TOut UseDefaultDbContext<TOut>(Func<BHLSqlDbContext, TOut> action)
        {
            return UseDbContext(action);
        }
        protected Task<TOut> UseDefaultDbContext<TOut>(Func<BHLSqlDbContext, Task<TOut>> action)
        {
            return UseDbContext(action);
        }
    }
}
