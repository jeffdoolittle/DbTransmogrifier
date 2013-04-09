using System.Linq;
using DbTransmogrifier.Logging;
using Xunit;

namespace DbTransmogrifier.Specs.using_processor_with_defaults
{
    public class when_executing_processor_with_null_arguments : ProcessorFixture
    {
        protected override string[] ArrangeArgs()
        {
            return null;
        }

        [Fact]
        public void then_error_is_displayed()
        {
            var lastLog = LogMessages.Last();
            Assert.Equal(LogLevel.Error, lastLog.Item1);
        }
    }
}