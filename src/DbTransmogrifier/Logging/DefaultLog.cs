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
            Info(string.Format(message, args));
        }

        public void Error (string message)
        {
            Console.WriteLine("[ERROR] - " + message);
        }

        public void ErrorFormat(string message, params object[] args)
        {
            Error(string.Format(message, args));
        }

        public void Fault(Exception ex)
        {
            Console.WriteLine("[FAULT] - " + ex);
        }
    }
}