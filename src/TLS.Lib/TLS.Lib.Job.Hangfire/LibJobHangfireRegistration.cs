using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
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
    public static class LibJobHangfireRegistration
    {
        private const string DefaultHangfireConfigKey = "Hangfire";

        /// <summary>
        /// Add lib AddHangfire use default config section ["Hangfire"]
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibJobHangfire(this IServiceCollection services)
        {
            return AddLibJobHangfire(services, m => {
            });
        }

        /// <summary>
        /// Add lib AddHangfire use default config section ["Hangfire"] with addition global configuration action
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupSettingsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibJobHangfire(this IServiceCollection services, Action<IGlobalConfiguration> setupSettingsAction)
        {
            // Use default section
            var configuration = services.GetConfiguration();
            var section = configuration.GetSection(DefaultHangfireConfigKey);
            if (!section.Exists())
            {
                throw new Exception($"Configuration section [{DefaultHangfireConfigKey}] was not found");
            }
            return AddLibJobHangfire(services, section, setupSettingsAction);
        }

        /// <summary>
        /// Add lib AddHangfire use special config section
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibJobHangfire(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            return AddLibJobHangfire(services, configurationSection, null);
        }

        /// <summary>
        /// Add lib AddHangfire use special config section with addition global configuration action
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection"></param>
        /// <param name="setupSettingsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibJobHangfire(this IServiceCollection services, IConfigurationSection configurationSection, Action<IGlobalConfiguration> setupSettingsAction)
        {
            // Check param
            if (configurationSection == null)
            {
                throw new ArgumentNullException(nameof(configurationSection));
            }

            // Check param
            if (!configurationSection.Exists())
            {
                throw new Exception($"Configuration section [{configurationSection.Key}] was not found");
            }

            // Connection
            var connectionString = configurationSection["Connection"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"[Connection] was not found in your configuration section [{configurationSection.Key}]");
            }

            var configStorageOptions = configurationSection.GetSection("StorageOptions");
            // Check param
            if (!configStorageOptions.Exists())
            {
                throw new Exception($"[StorageOptions] section was not found in your configuration section [{configurationSection.Key}]");
            }

            // Get storage options
            var storageOptions = configStorageOptions.Get<SqlServerStorageOptions>();
            return AddLibJobHangfireInternal(services, connectionString, storageOptions, setupSettingsAction);
        }

        /// <summary>
        /// Add lib AddHangfire use special connection string and Sql Server Storage Options
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="setupStorageAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibJobHangfire(this IServiceCollection services, string connectionString, Action<SqlServerStorageOptions> setupStorageAction)
        {
            return AddLibJobHangfire(services, connectionString, setupStorageAction, null);
        }

        /// <summary>
        /// Add lib AddHangfire use special connection string and Sql Server Storage Options with addition global configuration action
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="setupStorageAction"></param>
        /// <param name="setupSettingsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibJobHangfire(this IServiceCollection services, string connectionString, Action<SqlServerStorageOptions> setupStorageAction, Action<IGlobalConfiguration> setupSettingsAction)
        {
            // Check param
            if (setupStorageAction == null)
            {
                throw new ArgumentNullException(nameof(setupStorageAction));
            }

            // Invoke setup storage options
            var storageOptions = new SqlServerStorageOptions();
            setupStorageAction(storageOptions);

            return AddLibJobHangfireInternal(services, connectionString, storageOptions, setupSettingsAction);
        }

        private static IServiceCollection AddLibJobHangfireInternal(this IServiceCollection services, string connectionString, SqlServerStorageOptions storageOptions, Action<IGlobalConfiguration> setupSettingsAction)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Check param
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            // Check param
            if (storageOptions == null)
            {
                throw new ArgumentNullException(nameof(storageOptions));
            }

            // If use connection key => get connection string use this key
            if (Regex.IsMatch(connectionString, @"^[0-9a-zA-Z\-_]+$"))
            {
                var configuration = services.GetConfiguration();
                var connectionStringValue = configuration.GetConnectionString(connectionString);
                if (!string.IsNullOrWhiteSpace(connectionStringValue))
                {
                    connectionString = connectionStringValue;
                }
                else
                {
                    throw new Exception($"Connection string was not found with key [{connectionString}]");
                }
            }

            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString, storageOptions);
            return services.AddHangfire(config => {
                //var storageOptions = configuration.GetSection("Hangfire:StorageOptions").Get<SqlServerStorageOptions>();
                //config.UseSqlServerStorage(connectionString, storageOptions);
                config.UseColouredConsoleLogProvider();
                config.UseConsole(); ;
                if (setupSettingsAction != null)
                {
                    setupSettingsAction.Invoke(config);
                }
            });
        }
        public static IServiceCollection AddLibJobHangfire(this IServiceCollection services, IConfiguration configuration, Action<IGlobalConfiguration> setting)
        {
            var connectionString = configuration.GetValue<string>("Hangfire:Connection");
            var storageOptions = configuration.GetSection("Hangfire:StorageOptions").Get<SqlServerStorageOptions>();
            if (Regex.IsMatch(connectionString, @"^[0-9a-zA-Z\-_]+$"))
            {
                connectionString = configuration.GetConnectionString(connectionString);
            }

            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString, storageOptions);

            return services.AddHangfire(config => {
                //var storageOptions = configuration.GetSection("Hangfire:StorageOptions").Get<SqlServerStorageOptions>();
                //config.UseSqlServerStorage(connectionString, storageOptions);
                config.UseColouredConsoleLogProvider();
                config.UseConsole();;
                if (setting != null)
                {
                    setting.Invoke(config);
                }    
            });
        }
        //public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, IConfiguration configuration)
        //{
        //    return AddLibJobHangfireAndServer(services, configuration, null, null);
        //}
        //public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, IConfiguration configuration, Action<IGlobalConfiguration> setting)
        //{
        //    return AddLibJobHangfireAndServer(services, configuration, setting, null);
        //}
        //public static IServiceCollection AddLibJobHangfireAndServer(this IServiceCollection services, IConfiguration configuration, Action<IGlobalConfiguration> setting, Action<BackgroundJobServerOptions> optionsAction)
        //{
        //    return services.AddLibJobHangfire(configuration, setting)
        //    .AddHangfireServer(m => {
        //        if (optionsAction != null)
        //        {
        //            optionsAction.Invoke(m);
        //        }
        //    });
        //}

        /// <summary>
        /// Use lib UseHangfireDashboard use default config section ["Hangfire"]
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLibJobHangfireDashboard(this IApplicationBuilder app)
        {
            // Use default section
            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(DefaultHangfireConfigKey);
            if (!section.Exists())
            {
                throw new Exception($"Configuration section [{DefaultHangfireConfigKey}] was not found");
            }
            return UseLibJobHangfireDashboard(app, section);
        }

        /// <summary>
        /// Use lib UseHangfireDashboard use special config section
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configurationSection"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLibJobHangfireDashboard(this IApplicationBuilder app, IConfigurationSection configurationSection)
        {
            // Check param
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Check param
            if (configurationSection == null)
            {
                throw new ArgumentNullException(nameof(configurationSection));
            }

            // Check param
            if (!configurationSection.Exists())
            {
                throw new Exception($"Configuration section [{configurationSection.Key}] was not found");
            }

            // DashboardTitle and DashboardPath
            var dashboardTitle = string.IsNullOrEmpty(configurationSection["DashboardTitle"]) ? "Job dashboard"
                : configurationSection["DashboardTitle"];
            var dashboardPath = string.IsNullOrEmpty(configurationSection["DashboardPath"]) ? "/hangfire"
                : string.Format("/{0}", configurationSection["DashboardPath"].TrimStart('/'));

            // Basic users authorization
            IList<BasicAuthAuthorizationUser> users = null;
            var usersSection = configurationSection.GetSection("Users");
            if (usersSection.Exists())
            {
                users = usersSection.Get<List<BasicAuthAuthorizationUser>>();
            }

            // Authorizations
            IEnumerable<IDashboardAuthorizationFilter> authorizations = null;
            if (users != null && users.Count > 0)
            {
                authorizations = new[]
                    {
                        new BasicAuthAuthorizationFilter(m => {
                            m.LoginCaseSensitive = true;
                            m.Users = users;
                            //m.AddUser("admin", Encoding.UTF8.GetBytes("admin"), true);
                        })
                    };
            }

            // UseHangfireDashboard
            return app.UseMiddleware<RedirectToHangfirePathMiddleware>()
                .UseMiddleware<FixHangfireConsoleMiddleware>()
                //.Map(new PathString(dashboardPath + "/js"), x => x.UseMiddleware<FixHangfireConsoleMiddleware>())
                .UseHangfireDashboard(dashboardPath, new DashboardOptions
                {
                    DashboardTitle = dashboardTitle,
                    Authorization = authorizations
                });
        }

        /// <summary>
        /// Use lib UseHangfireDashboard use dashboard title and dashboard path (default is /hangfire)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="dashboardTitle"></param>
        /// <param name="dashboardPath"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLibJobHangfireDashboard(this IApplicationBuilder app, string dashboardTitle, string dashboardPath = "/hangfire")
        {
            return UseLibJobHangfireDashboard(app, null, dashboardTitle, dashboardPath);
        }

        /// <summary>
        /// Use lib UseHangfireDashboard use basic authAuthorization (ex: setting user/pass ...) action
        /// </summary>
        /// <param name="app"></param>
        /// <param name="setupBasicAuthorizationOptions"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLibJobHangfireDashboard(this IApplicationBuilder app, Action<BasicAuthAuthorizationFilterOptions> setupBasicAuthorizationOptions)
        {
            return UseLibJobHangfireDashboard(app, setupBasicAuthorizationOptions, null, null);
        }

        /// <summary>
        /// Use lib UseHangfireDashboard use basic authAuthorization (ex: setting user/pass ...) action and dashboard title and dashboard path (default is /hangfire)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="setupBasicAuthorizationOptions"></param>
        /// <param name="dashboardTitle"></param>
        /// <param name="dashboardPath"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLibJobHangfireDashboard(this IApplicationBuilder app, Action<BasicAuthAuthorizationFilterOptions> setupBasicAuthorizationOptions, string dashboardTitle, string dashboardPath = "/hangfire")
        {
            // Check param
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // DashboardTitle and DashboardPath
            dashboardTitle = string.IsNullOrEmpty(dashboardTitle) ? "Job dashboard" : dashboardTitle;
            dashboardPath = string.IsNullOrEmpty(dashboardPath) ? "/hangfire" : string.Format("/{0}", dashboardPath.TrimStart('/'));

            // Authorizations
            IEnumerable<IDashboardAuthorizationFilter> authorizations = null;
            if (setupBasicAuthorizationOptions != null)
            {
                authorizations = new[] { new BasicAuthAuthorizationFilter(setupBasicAuthorizationOptions) };
            }

            // UseHangfireDashboard
            return app.UseMiddleware<RedirectToHangfirePathMiddleware>()
                .UseMiddleware<FixHangfireConsoleMiddleware>()
                //.Map(new PathString(dashboardPath + "/js"), x => x.UseMiddleware<FixHangfireConsoleMiddleware>())
                .UseHangfireDashboard(dashboardPath, new DashboardOptions
                {
                    DashboardTitle = dashboardTitle,
                    Authorization = authorizations
                });
        }
    }
}
