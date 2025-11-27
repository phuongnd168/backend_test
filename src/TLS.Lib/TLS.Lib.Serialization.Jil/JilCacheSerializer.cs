using Jil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Caching;
using TLS.Core.Caching.Redis;

namespace TLS.Lib.Serialization.Jil
{
    public class JilCacheSerializer : JilSerializer, ICacheSerializer
    {
        public JilCacheSerializer(DateTimeFormat? dateFormat, Encoding encoding) : base (dateFormat, encoding)
        {

        }
    }
}
