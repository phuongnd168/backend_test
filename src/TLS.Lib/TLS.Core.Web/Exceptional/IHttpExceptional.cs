using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Exceptional
{
    public interface IHttpExceptional : IExceptional
    {
        void Log(Exception ex, HttpContext context, string category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null);
        Task LogAsync(Exception ex, HttpContext context, string category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null);
    }
}
