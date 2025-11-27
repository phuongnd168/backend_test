using EasyNetQ;
using EasyNetQ.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TLS.Core;
using TLS.Core.Messaging;
using TLS.Lib.Messaging.RabbitMQ.Log;

namespace TLS.Lib.Messaging.RabbitMQ
{
    public static class LibMessagingRabbitMQRegistration
    {
        private const string DefaultListConnectionsKey = "RabbitMQ:Connections";
        private const string DefaultDependencyInjectionConnectionKey = "Default";

        /// <summary>
        /// Add lib RegisterEasyNetQ use default connection ["RabbitMQ:Connections:Default"]
        /// </summary>
        /// <param name="services">required</param>
        /// <returns></returns>
        public static IServiceCollection AddLibMessagingRabbitMQ(this IServiceCollection services)
        {
            // Use default connection
            var connectionKey = $"{DefaultListConnectionsKey}:{DefaultDependencyInjectionConnectionKey}";
            var configuration = services.GetConfiguration();
            var connectionString = configuration[connectionKey];
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception($"RabbitMQ connection string was not found with key [{connectionKey}]");
            }
            return AddLibMessagingRabbitMQ(services, connectionString);
        }

        /// <summary>
        /// Add lib RegisterEasyNetQ with special connection string with action register services
        /// </summary>
        /// <param name="services">required</param>
        /// <param name="connectionString">required</param>
        /// <returns></returns>
        public static IServiceCollection AddLibMessagingRabbitMQ(this IServiceCollection services, string connectionString)
        {
            return AddLibMessagingRabbitMQ(services, connectionString, null);
        }

        /// <summary>
        /// Add lib RegisterEasyNetQ with special connection string with action register services
        /// </summary>
        /// <param name="services">required</param>
        /// <param name="connectionString">required</param>
        /// <param name="actionRegisterServices">options</param>
        /// <returns></returns>
        public static IServiceCollection AddLibMessagingRabbitMQ(this IServiceCollection services, string connectionString, Action<IServiceRegister> actionRegisterServices)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Check param
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            // If use connection key => get connection string use this key
            if (Regex.IsMatch(connectionString, @"^[0-9a-zA-Z\-_]+$"))
            {
                var connectionKey = $"{DefaultListConnectionsKey}:{connectionString}";
                var configuration = services.GetConfiguration();
                var connectionStringValue = configuration[connectionKey];
                if (!string.IsNullOrWhiteSpace(connectionStringValue))
                {
                    connectionString = connectionStringValue;
                }
                else
                {
                    throw new Exception($"RabbitMQ connection string was not found with key [{connectionKey}]");
                }
            }

            return AddLibMessagingRabbitMQInternal(services, connectionString, null, actionRegisterServices);
        }

        /// <summary>
        /// Add lib RegisterEasyNetQ use connection configuration factory action
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionConfigurationFactory"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibMessagingRabbitMQ(this IServiceCollection services, Func<IServiceResolver, ConnectionConfiguration> connectionConfigurationFactory)
        {
            return AddLibMessagingRabbitMQ(services, connectionConfigurationFactory, null);
        }

        /// <summary>
        /// Add lib RegisterEasyNetQ use connection configuration factory action  with action register services
        /// </summary>
        /// <param name="services">required</param>
        /// <param name="connectionConfigurationFactory">required</param>
        /// <param name="actionRegisterServices">options</param>
        /// <returns></returns>
        public static IServiceCollection AddLibMessagingRabbitMQ(this IServiceCollection services, Func<IServiceResolver, ConnectionConfiguration> connectionConfigurationFactory, Action<IServiceRegister> actionRegisterServices)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Check param
            if (connectionConfigurationFactory == null)
            {
                throw new ArgumentNullException(nameof(connectionConfigurationFactory));
            }

            return AddLibMessagingRabbitMQInternal(services, null, connectionConfigurationFactory, actionRegisterServices);
        }

        private static IServiceCollection AddLibMessagingRabbitMQInternal(this IServiceCollection services, string connectionString, Func<IServiceResolver, ConnectionConfiguration> connectionConfigurationFactory, Action<IServiceRegister> actionRegisterServices)
        {
            // Log provider
            //EasyNetQ.Logging.LogProvider.SetCurrentLogProvider(new QueueLogProvider());

            #region services
            // Make IOptions<ExceptionalSettings> available for injection everywhere
            services.AddSingleton<IQueueMetadata, QueueMetadata>();
            services.AddSingleton<IRabbitMQService, RabbitMQService>();
            services.AddSingleton<IRabbitMQFactory, RabbitMQFactoryService>();

            //var sss = configuration.GetValue<string>("RabbitMQ:Connections:Default");
            //Serilog.Log.Logger.Information("RabbitMQ Connection value 1 {0}", sss);
            //Serilog.Log.ForContext("SourceContext", "LibMessagingRabbitMQ", false).Information("RabbitMQ Connection value 2 {0}", sss);

            //services.RegisterEasyNetQ(configuration.GetValue<string>("RabbitMQ:Connections:Default"), registerServices =>
            if (!string.IsNullOrEmpty(connectionString))
            {
                services.RegisterEasyNetQ(connectionString, registServices =>
                {
                    if (actionRegisterServices != null)
                    {
                        actionRegisterServices(registServices);
                    }
                    registServices.Register<IConventions, QueueConventions>();
                });
            }  
            else
            {
                services.RegisterEasyNetQ(connectionConfigurationFactory, registServices =>
                {
                    if (actionRegisterServices != null)
                    {
                        actionRegisterServices(registServices);
                    }
                    registServices.Register<IConventions, QueueConventions>();
                });
            }    
            
            #endregion
            return services;
        }
    }
}
