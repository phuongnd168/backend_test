using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Exceptional;
using StackExchange.Exceptional;

namespace TLS.Lib.Exceptional
{
    public class StackExchangeHttpExceptional : StackExchangeExceptional, IHttpExceptional
    {
        public void Log(Exception ex, HttpContext context, string category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null)
        {
            AspNetCoreExtensions.Log(ex, context, category, rollupPerServer, customData, applicationName);
        }

        public async Task LogAsync(Exception ex, HttpContext context, string category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null)
        {
            await AspNetCoreExtensions.LogAsync(ex, context, category, rollupPerServer, customData, applicationName);
        }
    }
}
