using EasyNetQ;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Messaging.RabbitMQ
{
    public class QueueMetadata : IQueueMetadata
    {
        private static readonly Dictionary<Type, QueueAttribute> _metadata = new Dictionary<Type, QueueAttribute>();

        public void RegistMessage<T>(string queueName, string exchangeName = null)
        {
            RegistMessage(typeof(T), queueName, exchangeName);
        }
        public void RegistMessage(Type type, string queueName, string exchangeName = null)
        {
            if (!_metadata.ContainsKey(type))
            {
                _metadata.Add(type, new QueueAttribute(queueName)
                {
                    ExchangeName = exchangeName
                });
            }
        }
        public QueueAttribute GetMessageMetadata<T>()
        {
            return GetMessageMetadata(typeof(T));
        }
        public QueueAttribute GetMessageMetadata(Type type)
        {
            if (_metadata.ContainsKey(type))
            {
                return _metadata[type];
            }
            return null;
        }
    }
}
