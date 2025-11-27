using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Job.Hangfire.Filters.Basic
{
    public class BasicAuthAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly BasicAuthAuthorizationFilterOptions _options;

        public BasicAuthAuthorizationFilter()
            : this(new BasicAuthAuthorizationFilterOptions())
        {
        }

        public BasicAuthAuthorizationFilter(BasicAuthAuthorizationFilterOptions options)
        {
            _options = options;
        }

        public BasicAuthAuthorizationFilter(Action<BasicAuthAuthorizationFilterOptions> config)
        {
            _options = new BasicAuthAuthorizationFilterOptions();
            config(_options);
        }

        public bool Authorize([NotNull] DashboardContext dashboardContext)
        {
            return Authorize(dashboardContext.GetHttpContext());
        }

        public bool Authorize(HttpContext httpContext)
        {
            if ((_options.SslRedirect == true) && !httpContext.Request.IsHttps)
            {
                httpContext.Response.OnStarting(state => {
                    var context = (HttpContext)state;
                    var uri = new UriBuilder("https", context.Request.Host.Host, 443, context.Request.Path.Value, context.Request.QueryString.Value);

                    httpContext.Response.StatusCode = 301;
                    httpContext.Response.Redirect(uri.ToString());

                    return Task.CompletedTask;
                }, httpContext);
                return false;
            }

            //if ((_options.RequireSsl == true) && (httpContext.Request.IsSecure == false))
            //{
            //    httpContext.Response.WriteAsync("Secure connection is required to access Hangfire Dashboard.").Wait();
            //    return false;
            //}

            string header = httpContext.Request.Headers["Authorization"];

            if (String.IsNullOrWhiteSpace(header) == false)
            {
                var authValues = AuthenticationHeaderValue.Parse(header);

                if ("Basic".Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase))
                {
                    string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
                    var parts = parameter.Split(':');

                    if (parts.Length > 1)
                    {
                        string login = parts[0];
                        string password = parts[1];

                        if ((string.IsNullOrWhiteSpace(login) == false) && (string.IsNullOrWhiteSpace(password) == false))
                        {
                            return _options
                                .Users
                                .Any(user => user.Validate(login, password, _options.LoginCaseSensitive))
                                   || Challenge(httpContext);
                        }
                    }
                }
            }

            return Challenge(httpContext);
        }

        private bool Challenge(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 401;
            httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");

            httpContext.Response.WriteAsync("Authentication is required.").Wait();

            return false;
        }
    }
}
