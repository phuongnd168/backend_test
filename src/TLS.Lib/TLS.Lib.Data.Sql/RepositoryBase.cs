using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace TLS.Lib.Data.SQL
{
    public abstract class RepositoryBase<T> : RepositoryBase where T : class, IRepository
    {
        private ILogger<T> _logger;
        protected ILogger<T> Logger
        {
            get
            {
                return _logger ??= ServiceProvider.GetRequiredService<ILogger<T>>();
            }
        }
        public RepositoryBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
    public abstract class RepositoryBase
    {
        private const string _defaultConnectionName = "Default";
        protected virtual string DefaultConnectionName
        {
            get
            {
                return _defaultConnectionName;
            }
        }
        private readonly IServiceProvider _serviceProvider;
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }
        private readonly IConfiguration _configuration;
        protected IConfiguration Configuration
        {
            get
            {
                return _configuration;
            }
        }
        private readonly IDbConnectionFactory _connectionFactory;
        protected IDbConnectionFactory ConnectionFactory
        {
            get
            {
                return _connectionFactory;
            }
        }
        private readonly string _connectionStringDefault;
        protected string ConnectionStringDefault
        {
            get
            {
                return _connectionStringDefault;
            }
        }
        protected RepositoryBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _configuration = serviceProvider.GetRequiredService<IConfiguration>();
            _connectionFactory = serviceProvider.GetRequiredService<IDbConnectionFactory>();
            _connectionStringDefault = _configuration.GetConnectionString(DefaultConnectionName);
        }
        public string GetConnectionString()
        {
            return GetConnectionString(null);
        }
        public string GetConnectionString(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return ConnectionStringDefault;
            }
            return _configuration.GetConnectionString(name);
        }
        protected async Task<IDbConnection> OpenConnectionAsync()
        {
            return await OpenConnectionAsync(null);
        }
        protected async Task<IDbConnection> OpenConnectionAsync(string connectionStringName)
        {
            //var conn = new SqlConnection(GetConnectionString(connectionStringName));
            //await conn.OpenAsync();
            //return conn;
            return await ConnectionFactory.OpenAsync(GetConnectionString(connectionStringName));
        }
        protected IDbConnection OpenConnection(string connectionStringName)
        {
            //var conn = new SqlConnection(GetConnectionString(connectionStringName));
            //conn.Open();
            //return conn;
            return ConnectionFactory.Open(GetConnectionString(connectionStringName));
        }
        protected async Task<T> WithDefaultConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            return await WithConnection<T>(null, getData);
        }
        // use for buffered queries that return a type
        protected async Task<T> WithConnection<T>(string connectionStringName, Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var conn = await OpenConnectionAsync(connectionStringName))
                {
                    return await getData(conn);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

        protected async Task WithDefaultConnection(Func<IDbConnection, Task> getData)
        {
            await WithConnection(null, getData);
        }
        // use for buffered queries that do not return a type
        protected async Task WithConnection(string connectionStringName, Func<IDbConnection, Task> getData)
        {
            try
            {
                using (var conn = await OpenConnectionAsync(connectionStringName))
                {
                    await getData(conn);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

        protected async Task<TResult> WithDefaultConnection<TRead, TResult>(Func<IDbConnection, Task<TRead>> getData, Func<TRead, Task<TResult>> process)
        {
            return await WithConnection<TRead, TResult>(null, getData, process);
        }
        //use for non-buffered queries that return a type
        protected async Task<TResult> WithConnection<TRead, TResult>(string connectionStringName, Func<IDbConnection, Task<TRead>> getData, Func<TRead, Task<TResult>> process)
        {
            try
            {
                using (var conn = await OpenConnectionAsync(connectionStringName))
                {
                    var data = await getData(conn);
                    return await process(data);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

        protected void UseDbContext<TContext>(Action<TContext> action) where TContext : DbContext
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                using (TContext service = scope.ServiceProvider.GetRequiredService<TContext>())
                {
                    action(service);
                }
            }
        }
        protected TOut UseDbContext<TContext, TOut>(Func<TContext,TOut> action) where TContext : DbContext
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                using (TContext service = scope.ServiceProvider.GetRequiredService<TContext>())
                {
                    return action(service);
                }
            }
        }
    }
}
