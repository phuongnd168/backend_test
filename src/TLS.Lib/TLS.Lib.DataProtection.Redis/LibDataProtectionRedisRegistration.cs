using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TLS.Core;
using TLS.Core.Application;

namespace TLS.Lib.DataProtection.Redis
{
    public static class LibDataProtectionRedisRegistration
    {
        private const string DefaultDataProtectionRedisConfigKey = "DataProtection:Redis";
        private const string DefaultRedisListConnectionsKey = "Redis:Connections";

        /// <summary>
        /// Add DataProtection use redis with default configuration section ["DataProtection:Redis"],
        /// this section must includes below properties
        /// - Connection: Connection string to connect to redis server
        /// - Keys(can empty): Redis cache key to store DataProtection key
        /// Notice that the config value is not reload on change
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibDataProtectionUseRedis(this IServiceCollection services)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var configuration = services.GetConfiguration();
            var section = configuration.GetSection(DefaultDataProtectionRedisConfigKey);
            if (!section.Exists())
            {
                throw new Exception($"Configuration section [{DefaultDataProtectionRedisConfigKey}] was not found");
            }

            return AddLibDataProtectionUseRedis(services, section);
        }

        /// <summary>
        /// Add DataProtection use redis with special section
        /// this section must includes below properties
        /// - Connection: Connection string to connect to redis server
        /// - Keys(can empty): Redis cache key to store DataProtection key
        /// Notice that the config value is not reload on change
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibDataProtectionUseRedis(this IServiceCollection services, IConfigurationSection configurationSection)
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

            // Apply config options
            var options = configurationSection.Get<DataProtectionRedisOptions>();
            return AddLibDataProtectionUseRedis(services, m => {
                m.Connection = options.Connection;
                m.Keys = options.Keys;
            });
        }

        /// <summary>
        /// Add DataProtection use redis with setup action
        /// - Connection: Connection string to connect to redis server
        /// - Keys(can empty): Redis cache key to store DataProtection key
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibDataProtectionUseRedis(this IServiceCollection services, Action<DataProtectionRedisOptions> setupAction)
        {
            // Check param
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            // Setup options
            var options = new DataProtectionRedisOptions();
            setupAction(options);

            if (string.IsNullOrEmpty(options.Connection))
            {
                throw new Exception("Redis connection string is null or empty");
            }

            // If use connection key => get connection string use this key
            if (Regex.IsMatch(options.Connection, @"^[0-9a-zA-Z\-_]+$"))
            {
                var connectionKey = $"{DefaultRedisListConnectionsKey}:{options.Connection}";
                var configuration = services.GetConfiguration();
                var connectionStringValue = configuration[connectionKey];
                if (!string.IsNullOrWhiteSpace(connectionStringValue))
                {
                    options.Connection = connectionStringValue;
                }
                else
                {
                    throw new Exception($"Redis connection string was not found with key [{connectionKey}]");
                }
            }

            if (string.IsNullOrEmpty(options.Keys))
            {
                var appSettings = services.GetApplicationSettings();
                options.Keys = $"DataProtection-{appSettings.Application}";
            }

            // Redis connect use connection
            var redis = ConnectionMultiplexer.Connect(options.Connection);
            services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, options.Keys);
            return services;
        }
    }
}
