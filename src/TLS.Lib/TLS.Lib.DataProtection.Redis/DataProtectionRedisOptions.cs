using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.DataProtection.Redis
{
    public class DataProtectionRedisOptions
    {
        public string Connection { get; set; }
        public string Keys { get; set; }
    }
}
