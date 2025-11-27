using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TLS.Core.Messaging
{
    public interface IRabbitMQService
    {
        void RegistMessage<T>(string queueName, string exchangeName = null);
        void RegistMessage(Type type, string queueName, string exchangeName = null);
        Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken = default);
        Task SubscribeAsync<T>(string subscriptionId, string topic, Func<T, CancellationToken, Task> onMessage, ushort? prefetchCount = null, CancellationToken cancellationToken = default);
    }
}
