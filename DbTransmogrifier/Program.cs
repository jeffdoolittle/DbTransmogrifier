namespace DbTransmogrifier
{
    class Program
    {
        static void Main(string[] args)
        {
            var transmogrifier = new Transmogrifier();
            transmogrifier.TearDown();
            transmogrifier.TearDown();
            transmogrifier.Init();
            transmogrifier.Init();
            transmogrifier.TearDown();
            transmogrifier.Init();
        }
    }
}
