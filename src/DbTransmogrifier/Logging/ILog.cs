using System;

namespace DbTransmogrifier.Logging
{
    public interface ILog
    {
        void Info(string message);
        void InfoFormat(string message, params object[] args);
        void Error(string message);
        void ErrorFormat(string message, params object[] args);
        void Fault(Exception ex);
    }
}