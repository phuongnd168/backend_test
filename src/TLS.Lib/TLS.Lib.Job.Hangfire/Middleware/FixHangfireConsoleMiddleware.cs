using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TLS.Lib.Job.Hangfire.Dashboard;

namespace TLS.Lib.Job.Hangfire.Middleware
{
    public class FixHangfireConsoleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FixHangfireConsoleMiddleware> _logger;
        private bool _isInvoked = false;

        public FixHangfireConsoleMiddleware(RequestDelegate next, ILogger<FixHangfireConsoleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Abort if empty path
            if (!context.Request.Path.HasValue || string.IsNullOrEmpty(context.Request.Path.Value))
            {
                await _next(context);
                return;
            }

            // Fix js
            if (context.Request.Path.ToString().StartsWith("/hangfire/js"))
            {
                try
                {
                    var bodyContent = await HijackNext(context);
                    var isValid = bodyContent.IndexOf("hangfire.config.consolePollUrl;") > 0;
                    if (!isValid)
                    {
                        await WriteResource(context.Response, Resource.ResizeMinJs).ConfigureAwait(false);
                        await WriteResource(context.Response, Resource.ScriptJs).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Middleware FixHangfireConsoleMiddleware invoke fix js fail: {0}", ex.Message);
                    if (!_isInvoked)
                    {
                        await _next(context);
                    }
                }
            }

            // Fix css
            else if (context.Request.Path.ToString().StartsWith("/hangfire/css"))
            {
                try
                {
                    var bodyContent = await HijackNext(context);
                    var isValid = bodyContent.IndexOf(".console .line") > 0;
                    if (!isValid)
                    {
                        await WriteResource(context.Response, Resource.StyleCss).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Middleware FixHangfireConsoleMiddleware invoke fix css fail: {0}", ex.Message);
                    if (!_isInvoked)
                    {
                        await _next(context);
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }
        private async Task WriteResource(HttpResponse response, string resource)
        {
            using (var resourceStream = GetType().Assembly.GetManifestResourceStream(resource))
            {
                await resourceStream.CopyToAsync(response.Body).ConfigureAwait(false);
            }
        }

        public async Task DoNext(HttpContext context)
        {
            await _next(context);
            _isInvoked = true;
        }
        public async Task<string> HijackNext(HttpContext context)
        {
            using (var swapStream = new MemoryStream())
            {
                var originalResponseBody = context.Response.Body;

                context.Response.Body = swapStream;

                await DoNext(context);

                swapStream.Seek(0, SeekOrigin.Begin);
                string bodyContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
                swapStream.Seek(0, SeekOrigin.Begin);

                await swapStream.CopyToAsync(originalResponseBody);
                context.Response.Body = originalResponseBody;

                return bodyContent;
            }
        }
    }
}
