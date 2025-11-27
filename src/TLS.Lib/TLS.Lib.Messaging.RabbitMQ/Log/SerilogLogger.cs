using EasyNetQ.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Messaging.RabbitMQ.Log
{
    internal class SerilogLogger
    {
        private readonly Serilog.ILogger _logger;
        internal SerilogLogger(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception, params object[] formatParameters)
        {
            var translatedLevel = TranslateLevel(logLevel);
            if (messageFunc == null)
            {
                return IsEnabled(translatedLevel);
            }

            if (!IsEnabled(translatedLevel))
            {
                return false;
            }

            if (exception != null)
            {
                LogException(translatedLevel, messageFunc, exception, formatParameters);
            }
            else
            {
                LogMessage(translatedLevel, messageFunc, formatParameters);
            }

            return true;
        }

        private bool IsEnabled(LogEventLevel logEventLevel)
        {
            return _logger.IsEnabled(logEventLevel);
        }

        private void LogMessage(LogEventLevel logEventLevel, Func<string> messageFunc, object[] formatParameters)
        {
            _logger.Write(logEventLevel, messageFunc(), formatParameters);
        }

        private void LogException(LogEventLevel logEventLevel, Func<string> messageFunc, Exception exception, object[] formatParameters)
        {
            _logger.Write(logEventLevel, exception, messageFunc(), formatParameters);
        }

        private static LogEventLevel TranslateLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Fatal:
                    return LogEventLevel.Fatal;
                case LogLevel.Error:
                    return LogEventLevel.Error;
                case LogLevel.Warn:
                    return LogEventLevel.Warning;
                case LogLevel.Info:
                    return LogEventLevel.Information;
                case LogLevel.Trace:
                    return LogEventLevel.Verbose;
                default:
                    return LogEventLevel.Debug;
            }
        }
    }
}
