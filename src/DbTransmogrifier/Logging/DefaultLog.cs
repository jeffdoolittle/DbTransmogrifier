using System;

namespace DbTransmogrifier.Logging
{
    public class DefaultLog : ILog
    {
        public void Info(string message)
        {
            Console.WriteLine("[INFO] - " + message);
        }

        public void InfoFormat(string message, params object[] args)
        {
            Console.WriteLine("[INFO] - " + message, args);
        }

        public void Error (string message)
        {
            Console.WriteLine("[ERROR] - " + message);
        }
    }
}