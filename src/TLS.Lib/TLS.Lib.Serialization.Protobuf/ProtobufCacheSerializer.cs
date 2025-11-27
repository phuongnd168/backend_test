using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Caching;

namespace TLS.Lib.Serialization.Protobuf
{
    public class ProtobufCacheSerializer : ProtobufSerializer, ICacheSerializer
    {
        public ProtobufCacheSerializer() : base ()
        {

        }
    }
}
