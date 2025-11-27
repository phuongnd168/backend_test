using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TLS.Core;
using TLS.Core.Caching;
using TLS.Core.Caching.Redis;

namespace TLS.Lib.Caching.RedisCache
{
    public static class LibCachingRedisRegistration
    {
        internal const string DefaultListConnectionConfigKey = "Redis:Connections";
        internal const string DefaultDistributedConnectionConfigKey = "Distributed";
        /// <summary>
        /// Add Distributed cache use StackExchangeRedisCache
        /// And apply connection string from configuration["Redis:Connections:Distributed"]
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDistributedCacheUseRedis(this IServiceCollection services)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // use default connection string
            return AddDistributedCacheUseRedis(services, m => {
                var connectionKey = $"{DefaultListConnectionConfigKey}:{DefaultDistributedConnectionConfigKey}";
                var connectionString = services.GetConfiguration()[connectionKey];
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception($"No redis connnection string found in configuration[\"{connectionKey}\"]");
                }
                m.Configuration = connectionString;
            });
        }

        /// <summary>
        /// Add Distributed cache use StackExchangeRedisCache
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddDistributedCacheUseRedis(this IServiceCollection services, Action<RedisCacheOptions> setupAction)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Check param
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            // Use StackExchangeRedisCache
            return services.AddStackExchangeRedisCache(setupAction);
        }

        /// <summary>
        /// Add lib RedisCache
        /// if useRedisAsDistributedCache = true => Invoke AddDistributedCacheUseRedis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="useRedisAsDistributedCache"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibRedisCache(this IServiceCollection services, bool useRedisAsDistributedCache = false, Action<RedisCacheOptions> setupAction = null)
        {
            // Add lib Redis cache service and factory
            services.AddTransient<RedisCacheService>();
            services.AddSingleton<IRedisCacheFactory, RedisCacheFactoryService>();

            // If use redis as distributedCache
            if (useRedisAsDistributedCache)
            {
                // If not given setupAction, use default distributed redis cache connection configuration key as connection string
                if (setupAction == null)
                {
                    setupAction = m =>
                    {
                        var connectionKey = $"{DefaultListConnectionConfigKey}:{DefaultDistributedConnectionConfigKey}";
                        var connectionString = services.GetConfiguration()[connectionKey];
                        if (string.IsNullOrEmpty(connectionString))
                        {
                            throw new Exception($"No redis connnection string found in configuration[\"{connectionKey}\"]");
                        }
                        m.Configuration = connectionString;
                    };
                }
                AddDistributedCacheUseRedis(services, setupAction);
            }

            return services;
        }
    }
}
