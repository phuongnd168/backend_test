using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Handlers;

namespace TLS.Lib.Job.Hangfire.Handlers
{
    public class HangfireJobHandlerBase<T> : JobHandlerBase<T> where T : class
    {
        private PerformContext Context { get; set; }
        public HangfireJobHandlerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected void InitContext(PerformContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Logger.LogInformation and PerformContext.WriteLine
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected void WriteLogInformation(string message, params object[] args)
        {
            //message = string.Format("{0}: {1}", typeof(T).Name.Split(".").Last(), message);
            Logger.LogInformation(message, args);
            if (Context != null)
            {
                Context.WriteLine(ConsoleTextColor.DarkYellow, message, args);
            }    
        }

        /// <summary>
        /// Logger.LogDebug and PerformContext.WriteLine
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected void WriteLogDebug(string message, params object[] args)
        {
            //message = string.Format("{0}: {1}", typeof(T).Name.Split(".").Last(), message);
            Logger.LogDebug(message, args);
            if (Context != null)
            {
                Context.WriteLine(ConsoleTextColor.DarkBlue, message, args);
            }
        }

        /// <summary>
        /// Logger.LogError and PerformContext.WriteLine
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected void WriteLogError(string message, params object[] args)
        {
            //message = string.Format("{0}: {1}", typeof(T).Name.Split(".").Last(), message);
            Logger.LogError(message, args);
            if (Context != null)
            {
                Context.WriteLine(ConsoleTextColor.DarkRed, message, args);
            }
        }
    }
}
