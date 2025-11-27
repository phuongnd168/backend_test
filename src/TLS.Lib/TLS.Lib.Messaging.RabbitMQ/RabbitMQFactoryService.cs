using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLS.Core.Messaging;

namespace TLS.Lib.Messaging.RabbitMQ
{
    public class RabbitMQFactoryService : IRabbitMQFactory
    {
        private readonly ILogger<RabbitMQFactoryService> _logger;

        public RabbitMQFactoryService(ILogger<RabbitMQFactoryService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Create RabbitMQ service instance
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IRabbitMQService CreateInstance(string connectionString)
        {
            throw new NotImplementedException();
        }

    }
}
