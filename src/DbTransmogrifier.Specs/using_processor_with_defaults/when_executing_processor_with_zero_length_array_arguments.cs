using System.Linq;
using DbTransmogrifier.Logging;
using Xunit;

namespace DbTransmogrifier.Specs.using_processor_with_defaults
{
    public class when_executing_processor_with_zero_length_array_arguments : ProcessorFixture
    {
        protected override string[] ArrangeArgs()
        {
            return new string[0];
        }

        [Fact]
        public void then_error_is_displayed()
        {
            var lastLog = LogMessages.Last();
            Assert.Equal(LogLevel.Error, lastLog.Item1);
            Assert.Equal("No action specified", lastLog.Item2);
        }
    }
}