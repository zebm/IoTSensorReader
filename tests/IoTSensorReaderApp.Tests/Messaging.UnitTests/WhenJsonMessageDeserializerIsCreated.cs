using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Tests.Messaging.UnitTests
{
    [TestFixture]
    public class WhenJsonMessageDeserializerIsCreated : JsonMessageDeserializerTest
    {
        [Test]
        public void ThenDeserializerIsNotNull()
        {
            var deserializer = Deserializer;

            Assert.That(deserializer, Is.Not.Null);
        }

        [Test]
        public void ThenCanCallDeserializeMethod()
        {
            var jsonMessage = CreateValidJsonMessage(123, SensorType.Temperature, 25.5);

            Assert.DoesNotThrow(() => Deserializer.Deserialize(jsonMessage));
        }
    }
}