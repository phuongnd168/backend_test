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
    public class StackExchangeExceptional : IExceptional
    {
        public void LogNoContext(Exception ex, string category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null)
        {
            StackExchange.Exceptional.Extensions.LogNoContext(ex, category, rollupPerServer, customData, applicationName);
        }
    }
}
