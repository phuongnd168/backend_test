using StackExchange.Exceptional.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Exceptional
{
    public static class ExceptionalExtensions
    {
        public static T AddLogData<T>(this T ex, string key, string value) where T : Exception
        {
            ex.Data[Constants.CustomDataKeyPrefix + key] = value ?? string.Empty;
            return ex;
        }
    }
}
