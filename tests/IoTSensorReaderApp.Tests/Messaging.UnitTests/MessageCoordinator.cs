using IoTSensorReaderApp.Messaging;
using IoTSensorReaderApp.Processing;
using NSubstitute;

namespace IoTSensorReaderApp.Tests.Messaging.UnitTests
{
    [TestFixture]
    public class MessageCoordinatorTest : MessagingTest
    {
        protected MessageCoordinator Coordinator;
        protected IEventHubConsumer MockConsumer;
        protected IMessageDeserializer MockDeserializer;
        protected IMessageProcessor MockProcessor;

        [SetUp]
        public void SetUp()
        {
            MockConsumer = Substitute.For<IEventHubConsumer>();
            MockDeserializer = Substitute.For<IMessageDeserializer>();
            MockProcessor = Substitute.For<IMessageProcessor>();

            Coordinator = new MessageCoordinator(MockConsumer, MockProcessor, MockDeserializer);
        }
    }
}