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
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ILogger<RabbitMQService> _logger;
        private IBus _bus;
        private IQueueMetadata _metadata;

        public RabbitMQService(ILogger<RabbitMQService> logger, IBus bus, IQueueMetadata metadata)
        {
            _logger = logger;
            _bus = bus;
            _metadata = metadata;
        }
        public void RegistMessage<T>(string queueName, string exchangeName = null)
        {
            _metadata.RegistMessage<T>(queueName, exchangeName);
        }

        public void RegistMessage(Type type, string queueName, string exchangeName = null)
        {
            _metadata.RegistMessage(type, queueName, exchangeName);
        }
        public async Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken = default)
        {
            await _bus.PubSub.PublishAsync<T>(message, m =>
            {
                m.WithTopic(topic);
            }, cancellationToken);
        }

        public Task SubscribeAsync<T>(string subscriptionId, string topic, Func<T, CancellationToken, Task> onMessage, ushort? prefetchCount = null, CancellationToken cancellationToken = default)
        {
            var res = _bus.PubSub.SubscribeAsync<T>(subscriptionId, onMessage, m =>
            {
                m.WithTopic(topic);
                //m.WithAutoDelete(false);
                if (prefetchCount.HasValue && prefetchCount.Value > 0)
                {
                    m.WithPrefetchCount(prefetchCount.Value);
                }
            }, cancellationToken);
            return res.AsTask();
        }
    }
}
