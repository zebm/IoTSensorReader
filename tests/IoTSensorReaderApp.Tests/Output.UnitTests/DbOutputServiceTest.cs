using IoTSensorReaderApp.Configuration;
using IoTSensorReaderApp.Output;
using NSubstitute;

namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class DbOutputServiceTest : OutputServiceTest
    {
        protected DbOutputService DbOutputService;
        protected IDbConfiguration MockDbConfiguration;

        [SetUp]
        public void SetUp()
        {
            base.SetUp();
            MockDbConfiguration = Substitute.For<IDbConfiguration>();
            MockDbConfiguration.ConnectionString.Returns("mongodb://localhost:27017");
            MockDbConfiguration.DatabaseName.Returns("testdb");
            MockDbConfiguration.CollectionName.Returns("testcollection");

            DbOutputService = new DbOutputService(MockDbConfiguration);
        }
    }
}