using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Messaging.RabbitMQ
{
    public interface IQueueMetadata
    {
        void RegistMessage<T>(string queueName, string exchangeName = null);
        void RegistMessage(Type type, string queueName, string exchangeName = null);
        QueueAttribute GetMessageMetadata<T>();
        QueueAttribute GetMessageMetadata(Type type);
    }
}
