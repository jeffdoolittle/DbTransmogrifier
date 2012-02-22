using System;

namespace DbTransmogrifier.Logging
{
    public static class LoggerFactory
    {
        public static ILog GetLoggerFor(Type type)
        {
            return new DefaultLog();
        }
    }
}