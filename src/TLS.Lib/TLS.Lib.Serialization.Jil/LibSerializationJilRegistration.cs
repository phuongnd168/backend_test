using Jil;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Caching;
using TLS.Core.Caching.Redis;
using TLS.Core.Serialization;

namespace TLS.Lib.Serialization.Jil
{
    public static class LibSerializationJilRegistration
    {
        public static IServiceCollection AddLibSerializationJil(this IServiceCollection services, bool useJilForClientCache, bool useJilForRedisCache = true, DateTimeFormat? dateFormat = null, Encoding encoding = null)
        {
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    //options.Configuration = "localhost:4455";
            //    options.Configuration = configuration.GetValue<string>("Redis:Connections:Default");

            //});
            services.AddSingleton<ISerializer>(m => {
                return new JilSerializer(dateFormat, encoding);
            });

            if (useJilForClientCache)
            {
                AddLibSerializationJilForCache(services);
            }

            if (useJilForRedisCache)
            {
                AddLibSerializationJilForRedis(services);
            }

            return services;
        }
        public static IServiceCollection AddLibSerializationJilForCache(this IServiceCollection services, DateTimeFormat? dateFormat = null, Encoding encoding = null)
        {
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    //options.Configuration = "localhost:4455";
            //    options.Configuration = configuration.GetValue<string>("Redis:Connections:Default");

            //});
            services.AddSingleton<ICacheSerializer>(m => {
                return new JilCacheSerializer(dateFormat, encoding);
            });

            return services;
        }
        public static IServiceCollection AddLibSerializationJilForRedis(this IServiceCollection services, DateTimeFormat? dateFormat = null, Encoding encoding = null)
        {
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    //options.Configuration = "localhost:4455";
            //    options.Configuration = configuration.GetValue<string>("Redis:Connections:Default");

            //});
            services.AddSingleton<IRedisCacheSerializer>(m => {
                return new JilRedisCacheSerializer(dateFormat, encoding);
            });

            return services;
        }
    }
}
