using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbTransmogrifier.Logging;

namespace DbTransmogrifier
{
    public class Processor
    {
        private readonly Transmogrifier _transmogrifier;
        private static readonly ILog Log = LoggerFactory.GetLoggerFor(typeof(Processor));
        private static readonly IList<IActionHandler> Handlers = new List<IActionHandler>();
        private readonly string[] _args;

        public Processor()
            : this(Environment.GetCommandLineArgs().Skip(1).ToArray(), new Transmogrifier())
        {
        }

        public Processor(string[] args, Transmogrifier transmogrifier)
        {
            _transmogrifier = transmogrifier;
            _args = args ?? new string[0];
            Handlers.Add(new InitHandler { Transmogrifier = _transmogrifier });
            Handlers.Add(new UpToLatestHandler { Transmogrifier = _transmogrifier });
            Handlers.Add(new UpToHandler { Transmogrifier = _transmogrifier });
            Handlers.Add(new DownToHandler { Transmogrifier = _transmogrifier });
            Handlers.Add(new TearDownHandler { Transmogrifier = _transmogrifier });
            Handlers.Add(new CurrentVersionHandler { Transmogrifier = _transmogrifier });
            Handlers.Add(new HelpActionHandler(HelpMessage));
            Handlers.Add(new ErrorActionHandler());
        }

        public void Process()
        {
            Handlers.First(x => x.CanHandle(_args)).Handle(_args);
        }

        private static string HelpMessage()
        {
            var handlers = Handlers.Where(x => x.Arg != null);
            var sb = new StringBuilder();
            sb.AppendLine("Options:");
            foreach (var handler in handlers)
            {
                sb.AppendLine(handler.Arg);
            }
            return sb.ToString();
        }

        private interface IActionHandler
        {
            string Arg { get; }
            bool CanHandle(string[] args);
            void Handle(string[] args);
        }

        private class HelpActionHandler : IActionHandler
        {
            private readonly Func<string> _helpMessage;

            public string Arg
            {
                get { return "--help"; }
            }

            public HelpActionHandler(Func<string> helpMessage)
            {
                _helpMessage = helpMessage;
            }

            public bool CanHandle(string[] args)
            {
                return args.Contains(Arg);
            }

            public void Handle(string[] args)
            {
                Console.WriteLine(_helpMessage());
            }
        }

        private class ErrorActionHandler : IActionHandler
        {
            public string Arg
            {
                get { return null; }
            }

            public bool CanHandle(string[] args)
            {
                return true;
            }

            public void Handle(string[] args)
            {
                if (args.Length == 0)
                    Log.Error("No action specified");
                else
                    Log.Error("Unknown action " + args[0]);
            }
        }

        private abstract class TransmogrifierHandler : IActionHandler
        {
            private readonly string _arg;

            protected TransmogrifierHandler(string arg)
            {
                _arg = arg;
            }

            public Transmogrifier Transmogrifier { protected get; set; }

            public string Arg { get { return _arg; } }

            public bool CanHandle(string[] args)
            {
                return args[0].Contains(_arg);
            }

            public abstract void Handle(string[] args);
        }

        private class InitHandler : TransmogrifierHandler
        {
            public InitHandler() : base("--init") { }

            public override void Handle(string[] args)
            {
                Transmogrifier.Init();
            }
        }

        private class UpToLatestHandler : TransmogrifierHandler
        {
            public UpToLatestHandler() : base("--up-to-latest") { }

            public override void Handle(string[] args)
            {
                Transmogrifier.UpToLatest();
            }
        }

        private class UpToHandler : TransmogrifierHandler
        {
            public UpToHandler() : base("--up-to=") { }

            public override void Handle(string[] args)
            {
                var version = int.Parse(args[0].Replace(Arg, ""));
                Transmogrifier.UpTo(version);
            }
        }

        private class DownToHandler : TransmogrifierHandler
        {
            public DownToHandler() : base("--down-to=") { }

            public override void Handle(string[] args)
            {
                var version = int.Parse(args[0].Replace(Arg, ""));
                Transmogrifier.DownTo(version);
            }
        }

        private class TearDownHandler : TransmogrifierHandler
        {
            public TearDownHandler() : base("--tear-down") { }

            public override void Handle(string[] args)
            {
                Transmogrifier.TearDown();
            }
        }

        private class CurrentVersionHandler : TransmogrifierHandler
        {
            public CurrentVersionHandler() : base("--current-version") { }

            public override void Handle(string[] args)
            {
                Log.InfoFormat("Current version is {0}", Transmogrifier.CurrentVersion);
            }
        }
    }
}