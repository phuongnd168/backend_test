
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Exceptional
{
    public static class ExceptionalExtensions
    {
        private static readonly object _lockService = new object();
        private static IExceptional _exceptionalService;
        private static IExceptional ExceptionalService
        {
            get
            {
                if (_exceptionalService == null)
                {
                    lock (_lockService)
                    {
                        if (_exceptionalService == null)
                        {
                            _exceptionalService = GetExceptionalService();
                        }
                    }
                }
                return _exceptionalService;
            }
        }
        private static IExceptional GetExceptionalService()
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                return serviceScope.ServiceProvider.GetRequiredService<IExceptional>();
            }
        }
        public static T AddLogData<T>(this T ex, string key, string value) where T : Exception
        {
            ex.Data[ExceptionalConstants.CustomDataKeyPrefix + key] = value ?? string.Empty;
            return ex;
        }
        public static void LogNoContext(this Exception ex, string category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null)
        {
            ExceptionalService.LogNoContext(ex, category, rollupPerServer, customData, applicationName);
        }
    }
}
