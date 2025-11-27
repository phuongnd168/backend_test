using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLS.Core.Service;
using AutoMapper;
using TLS.Core.Handlers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TLS.Core.Web.Handlers
{
    public abstract class RequestHandlerBase<T> : CommonHandlerBase<T> where T : class
    {
        private IMapper _mapper;
        protected IMapper Mapper
        {
            get
            {
                return _mapper ??= ServiceProvider.GetService<IMapper>();
            }
        }
        private IHttpContextAccessor _httpContextAccessor;
        private IHttpContextAccessor HttpContextAccessor
        {
            get
            {
                return _httpContextAccessor ??= ServiceProvider.GetService<IHttpContextAccessor>();
            }
        }
        protected HttpContext HttpContext
        {
            get
            {
                return HttpContextAccessor.HttpContext;
            }
        }
        protected HttpRequest Request
        {
            get
            {
                return HttpContext.Request;
            }
        }
        protected CancellationToken RequestAborted
        {
            get
            {
                return HttpContext.RequestAborted;
            }
        }
        protected HttpResponse Response
        {
            get
            {
                return HttpContext.Response;
            }
        }
        protected ClaimsPrincipal User
        {
            get
            {
                return HttpContext.User;
            }
        }
        protected ISession Session
        {
            get
            {
                return HttpContext.Session;
            }
        }
        public RequestHandlerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
