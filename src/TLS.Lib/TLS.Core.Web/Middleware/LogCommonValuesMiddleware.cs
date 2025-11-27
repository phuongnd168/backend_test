using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Web.Middleware
{
    public class LogCommonValuesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogCommonValuesMiddleware> _logger;
        //private readonly IHttpContextAccessor _contextAccessor;

        public LogCommonValuesMiddleware(RequestDelegate next, ILogger<LogCommonValuesMiddleware> logger/*, IHttpContextAccessor contextAccessor*/)
        {
            this._next = next;
            this._logger = logger;
            //this._contextAccessor = contextAccessor;
        }

        public Task Invoke(HttpContext context)
        {
            IDisposable handle = null;
            try
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    //LogContext.PushProperty("UserName", context.User.Identity.Name);
                }

                var ipAddress = context.Connection.RemoteIpAddress.ToString() ?? null;
                var scope = new Dictionary<string, object> { ["ClientIP"] = ipAddress };
                handle = _logger.BeginScope(scope);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Middleware to log Client IP address invoke fail: {0}", ex.Message);
            }

            if (handle != null)
            {
                using (handle)
                {
                    return _next(context);
                }
            }
            else
            {
                return _next(context);
            }
        }
    }
}
