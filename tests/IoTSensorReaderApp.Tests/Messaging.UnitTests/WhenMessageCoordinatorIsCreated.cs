using IoTSensorReaderApp.Messaging;

namespace IoTSensorReaderApp.Tests.Messaging.UnitTests
{
    [TestFixture]
    public class WhenMessageCoordinatorIsCreated : MessageCoordinatorTest
    {
        [Test]
        public void ThenCoordinatorIsNotNull()
        {
            var coordinator = Coordinator;

            Assert.That(coordinator, Is.Not.Null);
        }

        [Test]
        public void ThenThrowsExceptionForNullConsumer()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MessageCoordinator(null!, MockProcessor, MockDeserializer);
            });
        }

        [Test]
        public void ThenThrowsExceptionForNullProcessor()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MessageCoordinator(MockConsumer, null!, MockDeserializer);
            });
        }

        [Test]
        public void ThenThrowsExceptionForNullDeserializer()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MessageCoordinator(MockConsumer, MockProcessor, null!);
            });
        }


    }
}