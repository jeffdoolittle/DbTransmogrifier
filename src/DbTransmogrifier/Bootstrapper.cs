using System;
using DbTransmogrifier.Logging;

namespace DbTransmogrifier
{
    public class Bootstrapper
    {
        public void Bootstrap()
        {
#if RELEASE
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