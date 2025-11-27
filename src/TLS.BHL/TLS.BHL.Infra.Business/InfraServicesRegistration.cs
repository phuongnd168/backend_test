using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Services;
using TLS.BHL.Infra.Business.Services;

namespace TLS.BHL.Infra.Business
{
    public static class InfraServiceRegistration
    {
        public static void AddInfraServices(this IServiceCollection services)
        {
            // Caching
            //services.AddSingleton<ICacheInstance, RedisCacheInstance>();
            //services.AddSingleton<ICacheService, RedisCacheService>();

            // Bus service
            //services.AddSingleton<IBusService, BusService>();

            #region BusinessServices
            services.AddScoped<IUserService, UserService>();
            // Other here
            #endregion
        }
    }
}
