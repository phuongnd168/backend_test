using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TLS.Core;
using TLS.Lib.Job.Hangfire.Filters.Basic;
using TLS.Lib.Job.Hangfire.Middleware;

namespace TLS.Lib.Job.Hangfire
{
    public static class LibJobHangfireAndServerRegistration
    {
        public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services)
        {
            Action<BackgroundJobServerOptions> setupServerAction = m =>
            {

            };
            return AddLibJobHangfireAndServer(services, setupServerAction, null);
        }
        public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, Action<BackgroundJobServerOptions> setupServerAction)
        {
            return AddLibJobHangfireAndServer(services, setupServerAction, null);
        }
        public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, Action<BackgroundJobServerOptions> setupServerAction, Action<IGlobalConfiguration> setupSettingsAction)
        {
            if (setupServerAction == null)
            {
                return services.AddLibJobHangfire(setupSettingsAction).AddHangfireServer();
            }
            return services.AddLibJobHangfire(setupSettingsAction).AddHangfireServer(setupServerAction);
        }


        public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            return AddLibJobHangfireAndServer(services, configurationSection, null, null);
        }
        public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, IConfigurationSection configurationSection, Action<BackgroundJobServerOptions> setupServerAction)
        {
            return AddLibJobHangfireAndServer(services, configurationSection, setupServerAction, null);
        }
        public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, IConfigurationSection configurationSection, Action<BackgroundJobServerOptions> setupServerAction, Action<IGlobalConfiguration> setupSettingsAction)
        {
            if (setupServerAction == null)
            {
                return services.AddLibJobHangfire(configurationSection, setupSettingsAction).AddHangfireServer();
            }
            return services.AddLibJobHangfire(configurationSection, setupSettingsAction).AddHangfireServer(setupServerAction);
        }


        public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, string connectionString, Action<SqlServerStorageOptions> setupStorageAction)
        {
            return AddLibJobHangfireAndServer(services, connectionString, setupStorageAction, null, null);
        }
        public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, string connectionString, Action<SqlServerStorageOptions> setupStorageAction, Action<BackgroundJobServerOptions> setupServerAction)
        {
            return AddLibJobHangfireAndServer(services, connectionString, setupStorageAction, setupServerAction, null);
        }
        public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, string connectionString, Action<SqlServerStorageOptions> setupStorageAction, Action<BackgroundJobServerOptions> setupServerAction, Action<IGlobalConfiguration> setupSettingsAction)
        {
            if (setupServerAction == null)
            {
                return services.AddLibJobHangfire(connectionString, setupStorageAction, setupSettingsAction).AddHangfireServer();
            }
            return services.AddLibJobHangfire(connectionString, setupStorageAction, setupSettingsAction).AddHangfireServer(setupServerAction);
        }
    }
}
