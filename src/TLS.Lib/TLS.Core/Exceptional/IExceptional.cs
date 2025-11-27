using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Exceptional
{
    public interface IExceptional
    {
        void LogNoContext(Exception ex, string category = null, bool rollupPerServer = false, Dictionary<string, string> customData = null, string applicationName = null);
    }
}
