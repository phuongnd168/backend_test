using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLS.Core;
using Microsoft.Extensions.DependencyInjection;

namespace TLS.Core.Caching
{
    public static class DistributedCacheExtensions
    {
        private static readonly object _lockSerializer = new object();
        private static ICacheSerializer _cacheSerializer;
        private static ICacheSerializer CacheSerializer
        {
            get
            {
                if (_cacheSerializer == null)
                {
                    lock (_lockSerializer)
                    {
                        if (_cacheSerializer == null)
                        {
                            _cacheSerializer = GetCacheSerializer();
                        }
                    }
                }
                return _cacheSerializer;
            }
        }
        private static ICacheSerializer GetCacheSerializer()
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                return serviceScope.ServiceProvider.GetRequiredService<ICacheSerializer>();
            }
        }

        //private static IDistributedCacheSerializationFormatter GetSerializationFormatter(SerializationFormat? serializationFormat)
        //{
        //    if (serializationFormat.HasValue && serializationFormat.Value == SerializationFormat.Json)
        //    {
        //        return DistributedCacheSerializationFormatterJson.Instance;
        //    }
        //    else if (serializationFormat.HasValue && serializationFormat.Value == SerializationFormat.Binary)
        //    {
        //        return DistributedCacheSerializationFormatterBinary.Instance;
        //    }
        //    else if (serializationFormat.HasValue && serializationFormat.Value == SerializationFormat.Xml)
        //    {
        //        return DistributedCacheSerializationFormatterXml.Instance;
        //    }
        //    else if (serializationFormat.HasValue && serializationFormat.Value == SerializationFormat.Protobuff)
        //    {
        //        return DistributedCacheSerializationFormatterProtobuf.Instance;
        //    }
        //    else
        //    {
        //        using (var serviceScope = ServiceActivator.GetScope())
        //        {
        //            return serviceScope.ServiceProvider.GetRequiredService<IDistributedCacheSerializationFormatter>();
        //        }
        //    }
        //}

        public static T GetObject<T>(this IDistributedCache distributedCache, string cacheKey)
            where T : class
        {
            var cached = distributedCache.Get(cacheKey);
            if (cached != null)
            {
                return CacheSerializer.Deserialize<T>(cached);
            }
            return default(T);
        }
        public static async Task<T> GetObjectAsync<T>(this IDistributedCache distributedCache, string cacheKey, CancellationToken token = default)
            where T : class
        {
            //var serializationFormatter = GetSerializationFormatter(serializationFormat);
            var cached = await distributedCache.GetAsync(cacheKey, token);
            if (cached != null)
            {
                return await CacheSerializer.DeserializeAsync<T>(cached);
            }
            return default(T);
        }
        public static void SetObject<T>(this IDistributedCache distributedCache, string cacheKey, T value, DateTimeOffset? absoluteExpiration = null, TimeSpan? slidingExpiration = null, DistributedCacheEntryOptions options = null)
        {
            if (options == null)
            {
                options = new DistributedCacheEntryOptions();
            }

            if (absoluteExpiration.HasValue)
            {
                options.SetAbsoluteExpiration(absoluteExpiration.Value);
            }
            if (slidingExpiration.HasValue)
            {
                options.SetSlidingExpiration(slidingExpiration.Value);
            }

            //var serializationFormatter = GetSerializationFormatter(serializationFormat);
            var dataBinary = CacheSerializer.Serialize(value);
            distributedCache.Set(cacheKey, dataBinary, options);
        }

        public static async Task SetObjectAsync<T>(this IDistributedCache distributedCache, string cacheKey, T value, DateTimeOffset? absoluteExpiration = null, TimeSpan? slidingExpiration = null, DistributedCacheEntryOptions options = null, CancellationToken token = default)
        {
            if (options == null)
            {
                options = new DistributedCacheEntryOptions();
            }

            if (absoluteExpiration.HasValue)
            {
                options.SetAbsoluteExpiration(absoluteExpiration.Value);
            }
            if (slidingExpiration.HasValue)
            {
                options.SetSlidingExpiration(slidingExpiration.Value);
            }

            //var serializationFormatter = GetSerializationFormatter(serializationFormat);
            var dataBinary = await CacheSerializer.SerializeAsync(value);
            await distributedCache.SetAsync(cacheKey, dataBinary, options, token);
        }
    }
}
