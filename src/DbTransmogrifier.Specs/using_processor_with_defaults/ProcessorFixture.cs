using System;
using System.Collections.Generic;
using DbTransmogrifier.Logging;
using DbTransmogrifier.Migrations;

namespace DbTransmogrifier.Specs.using_processor_with_defaults
{
    public abstract class ProcessorFixture
    {
        protected Transmogrifier Transmogrifier;
        protected Processor ClassUnderTest;
        protected IList<Tuple<LogLevel, string>> LogMessages = new List<Tuple<LogLevel, string>>();

        protected ProcessorFixture()
        {
            LoggerFactory.RegisterAfterLogInterceptor((type, logLevel, message) => LogMessages.Add(new Tuple<LogLevel, string>(logLevel, message)));
            ArrangeArgs();
            Act();
        }

        protected abstract string[] ArrangeArgs();

        protected void Act()
        {
            MigrationConfigurer.MigrationBuilderFactory = dependencies => new DefaultMigrationBuilder(dependencies, MigrationConfigurer.MigrationSourceFactory());
            MigrationConfigurer.MigrationSourceFactory = () => new DefaultMigrationTypeSource();
            MigrationConfigurer.ProviderNameSource = () => "System.Data.SqlClient";
            MigrationConfigurer.MasterConnectionStringSource = () => "";
            MigrationConfigurer.TargetConnectionStringSource = () => "";

            Transmogrifier = MigrationConfigurer
                .Configure()
             .BuildTransmogrifier();

            ClassUnderTest = new Processor(Transmogrifier, ArrangeArgs());
            ClassUnderTest.Process();
        }
    }
}
