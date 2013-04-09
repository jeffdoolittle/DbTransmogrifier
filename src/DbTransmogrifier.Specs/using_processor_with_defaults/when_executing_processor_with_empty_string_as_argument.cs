using System.Linq;
using DbTransmogrifier.Logging;
using Xunit;

namespace DbTransmogrifier.Specs.using_processor_with_defaults
{
    public class when_executing_processor_with_empty_string_as_argument : ProcessorFixture
    {
        protected override string[] ArrangeArgs()
        {
            return new[] { "" };
        }

        [Fact]
        public void then_error_is_displayed()
        {
            var lastLog = LogMessages.Last();
            Assert.Equal(LogLevel.Error, lastLog.Item1);
        }
    }
}