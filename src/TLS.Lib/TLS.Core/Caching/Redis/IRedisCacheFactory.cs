using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Caching.Redis
{
    public interface IRedisCacheFactory
    {
        /// <summary>
        /// Create redis cache instance from connection string
        /// </summary>
        /// <param name="connectionString">connection key (configuration key ["Redis:Connections:{connectionString}"]) or connection string</param>
        /// <returns>Redis cache instance</returns>
        IRedisCacheService CreateInstance(string connectionString);

        /// <summary>
        /// Create redis cache instance from connection string with special datatabase
        /// </summary>
        /// <param name="connectionString">connection key (configuration key ["Redis:Connections:{connectionString}"]) or connection string</param>
        /// <param name="database"></param>
        /// <returns>Redis cache instance</returns>
        IRedisCacheService CreateInstance(string connectionString, int? database);
    }
}
