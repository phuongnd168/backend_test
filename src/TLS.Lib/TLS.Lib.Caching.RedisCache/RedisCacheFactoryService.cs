using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TLS.Core.Caching;
using TLS.Core.Caching.Redis;

namespace TLS.Lib.Caching.RedisCache
{
    public class RedisCacheFactoryService : IRedisCacheFactory
    {
        private readonly IServiceProvider _serviceProvider;
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }
        private readonly IConfiguration _configuration;
        protected IConfiguration Configuration
        {
            get
            {
                return _configuration;
            }
        }
        public RedisCacheFactoryService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        /// <summary>
        /// Create redis cache service instance (use database number = 0)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IRedisCacheService CreateInstance(string connectionString)
        {
            return CreateInstance(connectionString, null);
        }

        /// <summary>
        ///  Create redis cache service instance with special database number
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public IRedisCacheService CreateInstance(string connectionString, int? database)
        {
            // If use connection key => get connection string use this key
            if (Regex.IsMatch(connectionString, @"^[0-9a-zA-Z\-_]+$"))
            {
                var connectionKey = $"{LibCachingRedisRegistration.DefaultListConnectionConfigKey}:{connectionString}";
                var connectionStringValue = Configuration[connectionKey];
                if (!string.IsNullOrWhiteSpace(connectionStringValue))
                {
                    connectionString = connectionStringValue;
                }
                else
                {
                    throw new Exception($"Redis connection string was not found with key [{connectionKey}]");
                }
            }

            var service = ServiceProvider.GetService<RedisCacheService>();
            service.Connect(connectionString, database);
            return service;
        }
    }
}
