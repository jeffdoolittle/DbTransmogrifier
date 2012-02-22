namespace DbTransmogrifier.Logging
{
    public interface ILog
    {
        void Info(string message);
        void InfoFormat(string message, params object[] args);
    }
}