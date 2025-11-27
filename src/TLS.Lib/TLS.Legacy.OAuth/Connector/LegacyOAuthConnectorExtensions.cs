using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core;

namespace TLS.Legacy.OAuth.Connector
{
    public static class LegacyOAuthConnectorExtensions
    {
        private const string DefaultOAuthConnectorConfigKey = "LegacyOAuthConnector";
        public static IServiceCollection AddLegacyConnector(this IServiceCollection services)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Use default section
            var configuration = services.GetConfiguration();
            var section = configuration.GetSection(DefaultOAuthConnectorConfigKey);
            if (!section.Exists())
            {
                throw new Exception($"Configuration section [{DefaultOAuthConnectorConfigKey}] was not found");
            }

            return AddLegacyConnector(services, section);
        }

        public static IServiceCollection AddLegacyConnector(this IServiceCollection services, Action<OAuthConnectorSettings> setupConnectorSettingsOptions)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Check param
            if (setupConnectorSettingsOptions == null)
            {
                throw new ArgumentNullException(nameof(setupConnectorSettingsOptions));
            }

            services.Configure(setupConnectorSettingsOptions);
            services.AddTransient<IOAuthConnectorService, OAuthConnectorService>();

            return services;
        }

        public static IServiceCollection AddLegacyConnector(this IServiceCollection services, IConfigurationSection configurationSection)
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

            services.Configure<OAuthConnectorSettings>(configurationSection);
            services.AddTransient<IOAuthConnectorService, OAuthConnectorService>();

            return services;
        }
    }
}
