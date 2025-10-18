using IoTSensorReaderApp.Configuration;

namespace IoTSensorReaderApp.E2ETests.Configuration
{
    public class TestAppConfiguration : IAppConfiguration
    {
        private readonly E2ETestConfiguration _config;

        public TestAppConfiguration(E2ETestConfiguration config)
        {
            _config = config;
        }

        public string IoTHubConnectionString => _config.IoTHubConnectionString;
    }
}