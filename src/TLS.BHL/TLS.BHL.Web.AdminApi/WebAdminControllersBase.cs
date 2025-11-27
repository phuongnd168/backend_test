using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Exceptional;
using TLS.Core.Exceptional;

namespace TLS.BHL.Web.AdminApi
{
    public abstract class WebAdminControllersBase<T> : ControllerBase where T : ControllerBase
    {
        private ILogger<T>? _logger;
        private IMediator? _mediator;
        private IHttpExceptional? _exceptional;
        private IConfiguration? _configuration;
        //private ICacheService? _cacheService;
        protected ILogger<T> Logger
        {
            get
            {
                _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();
                if (_logger != null)
                {
                    return _logger;
                }
                throw new InvalidOperationException("Can not resolve " + typeof(ILogger<T>).Name);
            }
        }
        protected IMediator Mediator
        {
            get
            {
                _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
                if (_mediator != null)
                {
                    return _mediator;
                }
                throw new InvalidOperationException("Can not resolve " + typeof(IMediator).Name);
            }
        }

        protected IHttpExceptional? Exceptional
        {
            get
            {
                _exceptional ??= HttpContext.RequestServices.GetService<IHttpExceptional>();
                if (_exceptional != null)
                {
                    return _exceptional;
                }
                throw new InvalidOperationException("Can not resolve " + typeof(IHttpExceptional).Name);
            }
        }

        protected IConfiguration? Configuration
        {
            get
            {
                _configuration ??= HttpContext.RequestServices.GetService<IConfiguration>();
                if (_configuration != null)
                {
                    return _configuration;
                }
                throw new InvalidOperationException("Can not resolve " + typeof(IConfiguration).Name);
            }
        }

        //protected ICacheService CacheService
        //{
        //    get
        //    {
        //        return _cacheService ??= HttpContext.RequestServices.GetService<ICacheService>();
        //    }
        //}

        protected TException LogError<TException>(TException ex, string? category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null)
            where TException : Exception
        {
            Exceptional?.Log(ex, HttpContext, category, rollupPerServer, customData, applicationName);
            return ex;
        }

        protected Exception LogError(string errorMessage, string? category = null, bool rollupPerServer = false, Dictionary<string, string>? customData = null, string? applicationName = null)
        {
            var ex = new Exception(errorMessage);
            Exceptional?.Log(ex, HttpContext, category, rollupPerServer, customData, applicationName);
            return ex;
        }

        protected Exception LogError(string errorMessage, Exception innerException, string? category = null, bool rollupPerServer = false, Dictionary<string, string>? customData = null, string? applicationName = null)
        {
            var ex = new Exception(errorMessage, innerException);
            Exceptional?.Log(ex, HttpContext, category, rollupPerServer, customData, applicationName);
            return ex;
        }

        protected Exception LogError(object inputError, Exception innerException, string? category = null, bool rollupPerServer = false, Dictionary<string, string>? customData = null, string? applicationName = null)
        {
            var errorMessage = $"Exception {innerException.Message} with input {JsonConvert.SerializeObject(inputError)}";
            return LogError(errorMessage, innerException, category, rollupPerServer, customData, applicationName);
        }
        /*
        protected async Task<TException> LogErrorAsync<TException>(TException ex, string? category = null, bool rollupPerServer = false, Dictionary<string, string>? customData = null, string? applicationName = null)
            where TException : Exception
        {

            await Exceptional?.LogAsync(ex, HttpContext, category, rollupPerServer, customData, applicationName);
            return ex;
        }

        protected async Task<Exception> LogErrorAsync(string errorMessage, string category = null, bool rollupPerServer = false, Dictionary<string, string>? customData = null, string applicationName = null)
        {
            var ex = new Exception(errorMessage);
            await Exceptional?.LogAsync(ex, HttpContext, category, rollupPerServer, customData, applicationName);
            return ex;
        }

        protected async Task<Exception> LogErrorAsync(string errorMessage, Exception innerException, string category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null)
        {
            var ex = new Exception(errorMessage, innerException);
            await Exceptional.LogAsync(ex, HttpContext, category, rollupPerServer, customData, applicationName);
            return ex;
        }
        */
        /*
        /// <summary>
        /// Current logged in user ID
        /// </summary>
        protected int? UserID
        {
            get
            {
                return User.GetIdentityUserId();
            }
        }

        /// <summary>
        /// Current logged in user name
        /// </summary>
        protected string UserName
        {
            get
            {
                return User.GetIdentityUserName();
            }
        }

        /// <summary>
        /// Current logged in user email
        /// </summary>
        protected string UserEmail
        {
            get
            {
                return User.GetIdentityUserEmail();
            }
        }

        /// <summary>
        /// Current logged in user name or email
        /// </summary>
        protected string UserNameOrEmail
        {
            get
            {
                if (UserID > 0)
                {
                    return UserName;
                }
                else
                {
                    return UserEmail;
                }
            }
        }
        */

        /// <summary>
        /// Trace identifier of current request
        /// </summary>
        protected string RequestID
        {
            get
            {
                return HttpContext.TraceIdentifier;
            }
        }

        /// <summary>
        /// Last 8 characters of RequestID
        /// </summary>
        protected string RequestIDShort
        {
            get
            {
                return RequestID.Substring(RequestID.Length - 8);
            }
        }
    }
}
