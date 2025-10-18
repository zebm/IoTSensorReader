using IoTSensorReaderApp.Configuration;
using IoTSensorReaderApp.Messaging;
using NSubstitute;
using NUnit.Framework;

namespace IoTSensorReaderApp.Tests.Messaging.UnitTests
{
    [TestFixture]
    public class WhenEventHubConsumerIsCreated : EventHubConsumerTest
    {
        [Test]
        public void ThenConsumerIsNotNull()
        {
            var consumer = Consumer;

            Assert.That(consumer, Is.Not.Null);
        }

        [Test]
        public void ThenCanCreateConsumerWithValidConfig()
        {
            var config = Substitute.For<IAppConfiguration>();
            config.IoTHubConnectionString.Returns("Endpoint=test-connection-string");

            Assert.DoesNotThrow(() => new EventHubConsumer(config));
        }
    }
}