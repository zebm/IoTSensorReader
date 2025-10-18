using IoTSensorReaderApp.Configuration;
using IoTSensorReaderApp.Messaging;
using NSubstitute;

namespace IoTSensorReaderApp.Tests.Messaging.UnitTests
{
    [TestFixture]
    public class EventHubConsumerTest: MessagingTest
    {
        protected EventHubConsumer Consumer;
        protected IAppConfiguration MockConfig;

        [SetUp]
        public void SetUp()
        {
            MockConfig = Substitute.For<IAppConfiguration>();
            MockConfig.IoTHubConnectionString.Returns("Endpoint-test-connection-string");
            Consumer = new EventHubConsumer(MockConfig);
        }
    }
}