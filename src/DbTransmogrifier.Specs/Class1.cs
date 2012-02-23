using DbTransmogrifier.Config;
using DbTransmogrifier.Dialects;
using Moq;
using Xunit;

namespace DbTransmogrifier.Specs
{    
    public class TransmogrifierFixture
    {
        protected Transmogrifier ClassUnderTest;
        protected AutoMock<Transmogrifier> AutoMock;

        public TransmogrifierFixture()
        {
            AutoMock = new AutoMock<Transmogrifier>();
            var configurator = AutoMock.GetMock<IConfigurator>();
            configurator.Setup(x => x.ProviderName).Returns("System.Data.SqlClient");
            AutoMock.GetMock<ISqlDialect>().Setup(x => x.ExtractDatabaseName(It.IsAny<string>())).Returns("FooBarDb");
            ClassUnderTest = AutoMock.CreateClassUnderTest();
        }

        [Fact]
        public void MyFirstTest()
        {
            AutoMock.GetMock<IConfigurator>().Verify(x=>x.ProviderName, Times.Once());
        }
    }

    //public abstract class DefaultMigrationResolverFixture
    //{
    //    protected DefaultMigrationResolverFixture()
    //    {
            
    //    }

    //    protected abstract void When();
    //}

    //public class when_resolving_migrations : DefaultMigrationResolverFixture
    //{
    //    public when_resolving_migrations()
    //    {
            
    //    }
    //}
}
