using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Exceptional
{
    public static class HttpExceptionalExtensions
    {
        private static readonly object _lockService = new object();
        private static IHttpExceptional _httpExceptionalService;
        private static IHttpExceptional HttpExceptionalService
        {
            get
            {
                if (_httpExceptionalService == null)
                {
                    lock (_lockService)
                    {
                        if (_httpExceptionalService == null)
                        {
                            _httpExceptionalService = GetHttpExceptionalService();
                        }
                    }
                }
                return _httpExceptionalService;
            }
        }
        private static IHttpExceptional GetHttpExceptionalService()
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                return serviceScope.ServiceProvider.GetRequiredService<IHttpExceptional>();
            }
        }
        public static void Log(this Exception ex, HttpContext context, string category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null)
        {
            HttpExceptionalService.Log(ex, context, category, rollupPerServer, customData, applicationName);
        }
        public static async Task LogAsync(this Exception ex, HttpContext context, string category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null)
        {
            await HttpExceptionalService.LogAsync(ex, context, category, rollupPerServer, customData, applicationName);
        }
    }
}
