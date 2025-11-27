using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Data;

namespace TLS.Lib.Data.SQL
{
    public static class LibDataSqlRegistration
    {
        public static void AddLibDataSQL(this IServiceCollection services)
        {
            #region repositories
            services.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();
            #endregion
        }
    }
}
