using IoTSensorReaderApp.Configuration;

namespace IoTSensorReaderApp.E2ETests.Configuration
{
    public class TestDbConfiguration : IDbConfiguration
    {
        private readonly E2ETestConfiguration _config;

        public TestDbConfiguration(E2ETestConfiguration config)
        {
            _config = config;
        }

        public string ConnectionString => _config.TestMongoDbConnectionString;
        public string DatabaseName => _config.TestDatabaseName;
        public string CollectionName => _config.TestCollectionName;
    }
}