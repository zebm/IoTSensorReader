using System;

namespace IoTSensorReaderApp.E2ETests.Configuration
{
    public class E2ETestConfiguration
    {
        public string IoTHubConnectionString =>
            Environment.GetEnvironmentVariable("IOT_HUB_CONNECTION_STRING")
            ?? throw new InvalidOperationException("IOT_HUB_CONNECTION_STRING not set for E2E tests");

        public string DeviceConnectionString =>
            Environment.GetEnvironmentVariable("DEVICE_CONNECTION_STRING")
            ?? throw new InvalidOperationException("DEVICE_CONNECTION_STRING not set for E2E tests");

        public string TestMongoDbConnectionString =>
            Environment.GetEnvironmentVariable("TEST_MONGODB_CONNECTION_STRING")
            ?? Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
            ?? "mongodb://localhost:27017";

        public string TestDatabaseName =>
            Environment.GetEnvironmentVariable("TEST_MONGODB_DATABASE_NAME")
            ?? "iotsensordata_e2e_tests";

        public string TestCollectionName =>
            Environment.GetEnvironmentVariable("TEST_MONGODB_COLLECTION_NAME")
            ?? "sensorreadings_test";

        public int TestTimeoutSeconds =>
            int.TryParse(Environment.GetEnvironmentVariable("E2E_TEST_TIMEOUT_SECONDS"), out int timeout)
                ? timeout : 60;

        public int MessageProcessingDelayMs =>
            int.TryParse(Environment.GetEnvironmentVariable("E2E_MESSAGE_PROCESSING_DELAY_MS"), out int delay)
                ? delay : 5000;
    }
}