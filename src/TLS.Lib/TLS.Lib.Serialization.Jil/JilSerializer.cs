using Jil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Caching;
using TLS.Core.Serialization;

namespace TLS.Lib.Serialization.Jil
{
    public class JilSerializer : ISerializer
    {
        public readonly Options JilOptions;
        // TODO: May make this configurable in the future.
        /// <summary>
        /// Encoding to use to convert string to byte[] and the other way around.
        /// </summary>
        /// <remarks>
        /// StackExchange.Redis uses Encoding.UTF8 to convert strings to bytes,
        /// hence we do same here.
        /// </remarks>
        private readonly Encoding Encoding = Encoding.UTF8;

        public JilSerializer(DateTimeFormat? dateFormat, Encoding encoding)
        {
            if (dateFormat.HasValue)
            {
                JilOptions = new Options(dateFormat: dateFormat.Value);
            }
            else
            {
                JilOptions = new Options(dateFormat: DateTimeFormat.ISO8601);
            }

            if (encoding != null)
            {
                Encoding = encoding;
            }
            else
            {
                Encoding = Encoding.UTF8;
            }
        }

        public byte[] Serialize(object item)
        {
            var jsonString = JSON.Serialize(item, JilOptions);
            return Encoding.GetBytes(jsonString);
        }

        public Task<byte[]> SerializeAsync(object item)
        {
            return Task.Factory.StartNew(() => Serialize(item));
        }

        public object Deserialize(byte[] serializedObject)
        {
            var jsonString = Encoding.GetString(serializedObject);
            return JSON.Deserialize(jsonString, typeof(object), JilOptions);
        }

        public object Deserialize(Type type, byte[] serializedObject)
        {
            var jsonString = Encoding.GetString(serializedObject);
            return JSON.Deserialize(jsonString, type, JilOptions);
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
            var jsonString = Encoding.GetString(serializedObject);
            return JSON.Deserialize<T>(jsonString, JilOptions);
        }

        public Task<T> DeserializeAsync<T>(byte[] serializedObject) where T : class
        {
            return Task.Factory.StartNew(() => Deserialize<T>(serializedObject));
        }
    }
}
