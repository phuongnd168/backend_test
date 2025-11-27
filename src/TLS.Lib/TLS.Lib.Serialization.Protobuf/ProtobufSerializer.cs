
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Caching;
using TLS.Core.Serialization;

namespace TLS.Lib.Serialization.Protobuf
{
    public class ProtobufSerializer : ISerializer
    {
        public ProtobufSerializer()
        {
        }

        public byte[] Serialize(object item)
        {
            if (item == null)
            {
                return null;
            }
            using (var stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, item);
                return stream.ToArray();
            }
        }

        public Task<byte[]> SerializeAsync(object item)
        {
            return Task.Factory.StartNew(() => Serialize(item));
        }

        public object Deserialize(byte[] serializedObject)
        {
            return Deserialize(typeof(object), serializedObject);
        }

        public object Deserialize(Type type, byte[] serializedObject)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (serializedObject == null)
            {
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }
                return null;
            }
            using (var stream = new MemoryStream(serializedObject))
            {
                return ProtoBuf.Serializer.Deserialize(type, stream);
            }
        }

        public Task<object> DeserializeAsync(byte[] serializedObject)
        {
            return Task.Factory.StartNew(() => Deserialize(serializedObject));
        }

        public Task<object> DeserializeAsync(Type type, byte[] serializedObject)
        {
            return Task.Factory.StartNew(() => Deserialize(type, serializedObject));
        }

        public T Deserialize<T>(byte[] serializedObject) where T : class
        {
            if (serializedObject == null)
            {
                return default(T);
            }
            using (var stream = new MemoryStream(serializedObject))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        }

        public Task<T> DeserializeAsync<T>(byte[] serializedObject) where T : class
        {
            return Task.Factory.StartNew(() => Deserialize<T>(serializedObject));
        }
    }
}
