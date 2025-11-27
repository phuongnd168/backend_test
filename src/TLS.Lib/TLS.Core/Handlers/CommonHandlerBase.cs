using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLS.Core.Service;

namespace TLS.Core.Handlers
{
    public abstract class CommonHandlerBase<T> : CommonHandlerBase where T : class
    {
        private ILogger<T> _logger;
        protected ILogger<T> Logger
        {
            get
            {
                return _logger ??= ServiceProvider.GetRequiredService<ILogger<T>>();
            }
        }
        public CommonHandlerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
    public abstract class CommonHandlerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SemaphoreSlim _serviceLock;
        private readonly IDictionary<Type, IService> _services;
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }
        public CommonHandlerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);
            _services = new Dictionary<Type, IService>();
        }

        protected T GetService<T>() where T : class, IService
        {
            var serviceKey = typeof(T);
            if (_services.ContainsKey(serviceKey))
            {
                return _services[serviceKey] as T;
            }
            _serviceLock.Wait();
            try
            {
                if (!_services.ContainsKey(serviceKey))
                {
                    _services[serviceKey] = ServiceProvider.GetRequiredService<T>();
                }
            }
            finally
            {
                _serviceLock.Release();
            }
            return _services[serviceKey] as T;
        }
    }
}
