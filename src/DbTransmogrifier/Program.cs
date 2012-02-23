namespace DbTransmogrifier
{
    class Program
    {
        static void Main()
        {
            new Bootstrapper().Bootstrap();
            new Processor().Process();
        }
    }

    public enum ExitCodes
    {
        Success = 0,
        Error = 1,
        Fault = 2
    }
}
