using System;

namespace DbTransmogrifier.Logging
{
    public class LogConfigurer
    {
        public static void Configure()
        {
#if !DEBUG
            LoggerFactory.RegisterBeforeLogInterceptor(                
                (type, logLevel, message) => (logLevel != LogLevel.Debug));
#endif

            LoggerFactory.RegisterAfterLogInterceptor(
                (type, logLevel, message) =>
                    {
                        if (logLevel == LogLevel.Error) Environment.ExitCode = (int)ExitCodes.Error;
                        if (logLevel == LogLevel.Fault) Environment.ExitCode = (int)ExitCodes.Fault;
                    });
        }
    }
}