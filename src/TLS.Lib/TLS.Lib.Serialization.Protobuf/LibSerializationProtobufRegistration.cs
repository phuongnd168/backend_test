using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Caching;
using TLS.Core.Serialization;

namespace TLS.Lib.Serialization.Protobuf
{
    public static class LibSerializationProtobufRegistration
    {
        public static IServiceCollection AddLibSerializationJil(this IServiceCollection services, bool useSerializationCache)
        {
            services.AddSingleton<ISerializer, ProtobufSerializer>();

            if (useSerializationCache)
            {
                return AddLibSerializationCacheJil(services);
            }

            return services;
        }
        public static IServiceCollection AddLibSerializationCacheJil(this IServiceCollection services)
        {
            services.AddSingleton<ICacheSerializer, ProtobufCacheSerializer>();

            return services;
        }
    }
}
