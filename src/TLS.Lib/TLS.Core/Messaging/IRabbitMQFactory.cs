using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TLS.Core.Messaging
{
    public interface IRabbitMQFactory
    {
        /// <summary>
        /// Create RabbitMQ service instance
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        IRabbitMQService CreateInstance(string connectionString);

        /// <summary>
        /// Not support connection configuration factory
        /// </summary>
        /// <param name="connectionConfigurationFactory"></param>
        /// <returns></returns>
        //IRabbitMQ CreateInstance(Action<ConnectionConfiguration> connectionConfigurationFactory);
    }
}
