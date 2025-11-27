using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Caching
{
    public interface IClientCacheFactory
    {
        /// <summary>
        /// Create client cache instance from connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>Client cache instance</returns>
        IClientCache CreateInstance(string connectionString);

        /// <summary>
        /// Create client cache instance from connection string with special datatabase (if client cache support multi database: ex Redis cache)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="database"></param>
        /// <returns>Client cache instance</returns>
        IClientCache CreateInstance(string connectionString, int? database);
    }
}
