using EasyNetQ;
using EasyNetQ.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Messaging.RabbitMQ
{
    public class QueueConventions : IConventions
    {
        private static readonly QueueAttribute DefaultMessageMetadata = new QueueAttribute(null);
        private readonly ITypeNameSerializer _typeNameSerializer;
        private readonly IQueueMetadata _metadata;

        public QueueConventions(ITypeNameSerializer typeNameSerializer, IQueueMetadata metadata)
        {
            _typeNameSerializer = typeNameSerializer;
            _metadata = metadata;
        }

        /// <summary>
        ///     Creates Conventions
        /// </summary>
        //public QueueConventions(ITypeNameSerializer typeNameSerializer, IQueueMetadata metadata)
        //{
        //    _typeNameSerializer = typeNameSerializer;
        //    _metadata = metadata;
        //    ExchangeNamingConvention = type =>
        //    {
        //        var attr = GetQueueAttribute(type, metadata);

        //        return string.IsNullOrEmpty(attr.ExchangeName)
        //            ? typeNameSerializer.Serialize(type)
        //            : attr.ExchangeName;
        //    };

        //    TopicNamingConvention = type => "";

        //    QueueNamingConvention = (type, subscriptionId) =>
        //    {
        //        var attr = GetQueueAttribute(type, metadata);

        //        if (string.IsNullOrEmpty(attr.QueueName))
        //        {
        //            var typeName = typeNameSerializer.Serialize(type);

        //            return string.IsNullOrEmpty(subscriptionId)
        //                ? typeName
        //                : $"{typeName}_{subscriptionId}";
        //        }

        //        return string.IsNullOrEmpty(subscriptionId)
        //            ? attr.QueueName
        //            : $"{attr.QueueName}_{subscriptionId}";
        //    };
        //    RpcRoutingKeyNamingConvention = typeNameSerializer.Serialize;

        //    ErrorQueueNamingConvention = receivedInfo => "EasyNetQ_Default_Error_Queue";
        //    ErrorExchangeNamingConvention = receivedInfo => "ErrorExchange_" + receivedInfo.RoutingKey;
        //    RpcRequestExchangeNamingConvention = type => "easy_net_q_rpc";
        //    RpcResponseExchangeNamingConvention = type => "easy_net_q_rpc";
        //    RpcReturnQueueNamingConvention = type => "easynetq.response." + Guid.NewGuid();

        //    ConsumerTagConvention = () => Guid.NewGuid().ToString();
        //}

        private QueueAttribute GetQueueAttribute(Type messageType, IQueueMetadata metadata)
        {
            // Get from queue message attribute
            var queueAttribute = messageType.GetAttribute<QueueAttribute>();
            if (queueAttribute != null)
            {
                return queueAttribute;
            }

            // Get from metadata of message
            var messageMetadata = metadata.GetMessageMetadata(messageType);
            if (messageMetadata != null)
            {
                return messageMetadata;
            }

            // Default
            return DefaultMessageMetadata;
        }

        //public ExchangeNameConvention ExchangeNamingConvention { get; set; }
        public ExchangeNameConvention ExchangeNamingConvention 
        { 
            get
            {
                return type => {
                    var attr = GetQueueAttribute(type, _metadata);

                    var exchangeName = string.IsNullOrEmpty(attr.ExchangeName) ? _typeNameSerializer.Serialize(type) : attr.ExchangeName;
                    return exchangeName;
                };
            }
        }

        /// <inheritdoc />
        //public TopicNameConvention TopicNamingConvention { get; set; }
        public TopicNameConvention TopicNamingConvention
        {
            get
            {
                return type => string.Empty;
            }
        }

        /// <inheritdoc />
        //public QueueNameConvention QueueNamingConvention { get; set; }
        public QueueNameConvention QueueNamingConvention
        {
            get
            {
                return (type, subscriptionId) =>
                {
                    var attr = GetQueueAttribute(type, _metadata);

                    if (string.IsNullOrEmpty(attr.QueueName))
                    {
                        var typeName = _typeNameSerializer.Serialize(type);

                        return string.IsNullOrEmpty(subscriptionId)
                            ? typeName
                            : $"{typeName}_{subscriptionId}";
                    }

                    var queueName = string.IsNullOrEmpty(subscriptionId)
                        ? attr.QueueName
                        : $"{attr.QueueName}_{subscriptionId}";
                    return queueName;
                };
            }
        }

        /// <inheritdoc />
        //public RpcRoutingKeyNamingConvention RpcRoutingKeyNamingConvention { get; set; }
        public RpcRoutingKeyNamingConvention RpcRoutingKeyNamingConvention
        {
            get
            {
                return _typeNameSerializer.Serialize;
            }
        }

        /// <inheritdoc />
        //public ErrorQueueNameConvention ErrorQueueNamingConvention { get; set; }
        public ErrorQueueNameConvention ErrorQueueNamingConvention
        {
            get
            {
                return receivedInfo => "EasyNetQ_Default_Error_Queue";
            }
        }

        /// <inheritdoc />
        //public ErrorExchangeNameConvention ErrorExchangeNamingConvention { get; set; }
        public ErrorExchangeNameConvention ErrorExchangeNamingConvention
        {
            get
            {
                return receivedInfo => "ErrorExchange_" + receivedInfo.RoutingKey;
            }
        }

        /// <inheritdoc />
        //public RpcExchangeNameConvention RpcRequestExchangeNamingConvention { get; set; }
        public RpcExchangeNameConvention RpcRequestExchangeNamingConvention
        {
            get
            {
                return type => "easy_net_q_rpc";
            }
        }

        /// <inheritdoc />
        //public RpcExchangeNameConvention RpcResponseExchangeNamingConvention { get; set; }
        public RpcExchangeNameConvention RpcResponseExchangeNamingConvention
        {
            get
            {
                return type => "easy_net_q_rpc";
            }
        }

        /// <inheritdoc />
        //public RpcReturnQueueNamingConvention RpcReturnQueueNamingConvention { get; set; }
        public RpcReturnQueueNamingConvention RpcReturnQueueNamingConvention
        {
            get
            {
                return type => "easynetq.response." + Guid.NewGuid();
            }
        }

        /// <inheritdoc />
        //public ConsumerTagConvention ConsumerTagConvention { get; set; }
        public ConsumerTagConvention ConsumerTagConvention
        {
            get
            {
                return () => Guid.NewGuid().ToString();
            }
        }

        public QueueTypeConvention QueueTypeConvention => throw new NotImplementedException();
    }
}
