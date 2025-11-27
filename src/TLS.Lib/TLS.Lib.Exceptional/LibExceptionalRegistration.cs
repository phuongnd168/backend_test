using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Application;
using TLS.Core.Configure;
using StackExchange.Exceptional;
using TLS.Core.Exceptional;
using TLS.Core;

namespace TLS.Lib.Exceptional
{
    public static class LibExceptionalRegistration
    {
        private const string DefaultExceptionalConfigKey = "Exceptional";

        /// <summary>
        /// Add lib StackExchangeExceptional use default config section ["Exceptional"]
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibExceptional(this IServiceCollection services)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Use default section
            var configuration = services.GetConfiguration();
            var section = configuration.GetSection(DefaultExceptionalConfigKey);
            if (!section.Exists())
            {
                throw new Exception($"Configuration section [{DefaultExceptionalConfigKey}] was not found");
            }

            return AddLibExceptional(services, section);
        }

        /// <summary>
        /// Add lib StackExchangeExceptional use special config section
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibExceptional(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
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

            // Make IOptions<ExceptionalSettings> available for injection everywhere
            services.AddExceptional(configurationSection, settings =>
            {
                var appSettings = services.GetApplicationSettings();
                settings.Store.ApplicationName = appSettings.Application;
                settings.UseExceptionalPageOnThrow = appSettings.Environment.IsDevelopment();
            });
            services.AddSingleton<IExceptional, StackExchangeExceptional>();
            services.AddSingleton<IHttpExceptional, StackExchangeHttpExceptional>();
            return services;
        }
    }
}
