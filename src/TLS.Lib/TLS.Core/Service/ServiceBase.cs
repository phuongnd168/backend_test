using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TLS.Core.Data;

namespace TLS.Core.Service
{
    public abstract class ServiceBase<T> : ServiceBase where T : class, IService
    {
        private ILogger<T> _logger;
        protected ILogger<T> Logger
        {
            get
            {
                return _logger ??= ServiceProvider.GetRequiredService<ILogger<T>>();
            }
        }
        public ServiceBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
    public abstract class ServiceBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SemaphoreSlim _repositoryLock, _serviceLock;
        private readonly IDictionary<Type, IRepository> _repositories;
        private readonly IDictionary<Type, IService> _services;
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }
        public ServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _repositoryLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);
            _serviceLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);
            _repositories = new Dictionary<Type, IRepository>();
            _services = new Dictionary<Type, IService>();
        }

        protected T GetRepository<T>() where T : class, IRepository
        {
            var repositoryKey = typeof(T);
            if (_repositories.ContainsKey(repositoryKey))
            {
                return _repositories[repositoryKey] as T;
            }
            _repositoryLock.Wait();
            try
            {
                if (!_repositories.ContainsKey(repositoryKey))
                {
                    _repositories[repositoryKey] = ServiceProvider.GetRequiredService<T>();
                }
            }
            finally
            {
                _repositoryLock.Release();
            }
            return _repositories[repositoryKey] as T;
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
