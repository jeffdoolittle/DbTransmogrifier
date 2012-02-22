namespace DbTransmogrifier
{
    class Program
    {
        static void Main(string[] args)
        {
            var transmogrifier = new Transmogrifier();
            transmogrifier.Init();
            transmogrifier.UpToLatest();
            transmogrifier.DownTo(0);
            transmogrifier.TearDown();
        }
    }
}
