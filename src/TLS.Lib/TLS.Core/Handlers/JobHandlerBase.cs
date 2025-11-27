using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLS.Core.Service;

namespace TLS.Core.Handlers
{
    public abstract class JobHandlerBase<T> : CommonHandlerBase<T> where T : class
    {
        public JobHandlerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
