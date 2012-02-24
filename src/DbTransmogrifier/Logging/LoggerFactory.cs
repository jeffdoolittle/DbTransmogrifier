using System;
using System.Collections.Generic;

namespace DbTransmogrifier.Logging
{
    public delegate bool BeforeLogInterceptor(Type type, LogLevel logLevel, string message);
    public delegate void AfterLogInterceptor(Type type, LogLevel logLevel, string message);

    public static class LoggerFactory
    {
        private static readonly IList<BeforeLogInterceptor> Before = new List<BeforeLogInterceptor>();
        private static readonly IList<AfterLogInterceptor> After = new List<AfterLogInterceptor>();

        public static void RegisterBeforeLogInterceptor(BeforeLogInterceptor interceptor)
        {
            Before.Add(interceptor);
        }

        public static void RegisterAfterLogInterceptor(AfterLogInterceptor interceptor)
        {
            After.Add(interceptor);
        }

        public static ILog GetLoggerFor(Type type)
        {
            return new InterceptingLogger(type, new DefaultLog());
        }

        private class InterceptingLogger : ILog
        {
            private readonly ILog _innerLogger;
            private readonly Type _type;

            public InterceptingLogger(Type type, ILog innerLogger)
            {
                _type = type;
                _innerLogger = innerLogger;
            }

            private void DoInterceptedLogging(LogLevel logLevel, string message, Action loggingAction)
            {
                foreach (var interceptor in Before)
                {
                    var intercepted = !interceptor.Invoke(_type, logLevel, message);
                    if (intercepted) return;
                }

                loggingAction();
                
                foreach (var interceptor in After) interceptor.Invoke(_type, logLevel, message);
            }

            public void Debug(string message)
            {
                DoInterceptedLogging(LogLevel.Debug, message, () => _innerLogger.Debug(message));
            }

            public void DebugFormat(string message, params object[] args)
            {
                DoInterceptedLogging(LogLevel.Debug, message, () => _innerLogger.DebugFormat(message, args));
            }

            public void Info(string message)
            {
                DoInterceptedLogging(LogLevel.Info, message, () => _innerLogger.Info(message));
            }

            public void InfoFormat(string message, params object[] args)
            {
                DoInterceptedLogging(LogLevel.Info, message, () => _innerLogger.InfoFormat(message, args));
            }

            public void Error(string message)
            {
                DoInterceptedLogging(LogLevel.Error, message, () => _innerLogger.Error(message));
            }

            public void ErrorFormat(string message, params object[] args)
            {
                DoInterceptedLogging(LogLevel.Error, message, () => _innerLogger.ErrorFormat(message, args));
            }

            public void Fault(Exception ex)
            {
                DoInterceptedLogging(LogLevel.Fault, ex.ToString(), () => _innerLogger.Fault(ex));
            }
        }
    }

    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warn = 2,
        Error = 3,
        Fault = 4
    }
}