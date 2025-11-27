using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Job.Hangfire.Middleware
{
    public class RedirectToHangfirePathMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RedirectToHangfirePathMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public RedirectToHangfirePathMiddleware(RequestDelegate next, ILogger<RedirectToHangfirePathMiddleware> logger, IConfiguration configuration)
        {
            this._next = next;
            this._logger = logger;
            this._configuration = configuration;
        }

        public Task Invoke(HttpContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(context.Request.Path) || context.Request.Path == "/")
                {
                    var isSingleApp = _configuration.GetValue<bool?>("Hangfire:IsSingleApp");
                    if (isSingleApp.HasValue && isSingleApp.Value)
                    {
                        var dashboardPath = _configuration.GetValue<string>("Hangfire:DashboardPath");
                        context.Response.Redirect(string.IsNullOrEmpty(dashboardPath) ? "/hangfire" : dashboardPath);
                        return Task.FromResult(0);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Middleware RedirectToHangfirePath invoke fail: {0} ({1})", ex.Message, ex.StackTrace);
            }
            return _next(context);
        }
    }
}
