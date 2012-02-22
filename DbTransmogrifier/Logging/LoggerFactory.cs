using System;

namespace DbTransmogrifier
{
    public static class LoggerFactory
    {
        public static ILog GetLoggerFor(Type type)
        {
            return new DefaultLog();
        }
    }

    public interface ILog
    {
        void Info(string message);
        void InfoFormat(string message, params object[] args);
    }

    public class DefaultLog : ILog
    {
        public void Info(string message)
        {
            Console.WriteLine("[INFO] - " + message);
        }

        public void InfoFormat(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }
    }
}