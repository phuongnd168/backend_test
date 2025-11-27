using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Repositories;
using TLS.BHL.Infra.Data.SQL;
using TLS.BHL.Infra.Data.SQL.Contexts;
using TLS.BHL.Infra.Data.SQL.Repositories;
using TLS.Core;
using TLS.Lib.Data.SQL;

namespace TLS.BHL.Infra.Data
{
    public static class InfraRepositoriesRegistration
    {
        public static void AddInfraRepositories(this IServiceCollection services)
        {
            // Apply use SQL server
            services.AddLibDataSQL();
            services.AddDbContext<BHLSqlDbContext>(options =>
            {
                options.UseSqlServer(services.GetConfiguration().GetConnectionString("BHL"), m => { 
                });
            });
            services.AddScoped<IBHLDbContext>(provider => provider.GetRequiredService<BHLSqlDbContext>());

            #region repositories
            services.AddScoped<IUserRepository, UserRepository>();
            // Other here
            #endregion
        }
    }
}
