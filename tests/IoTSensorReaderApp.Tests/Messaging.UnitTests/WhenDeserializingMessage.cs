using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Tests.Messaging.UnitTests
{
    [TestFixture]
    public class WhenDeserializingMessage : JsonMessageDeserializerTest
    {
        [Test]
        public void ThenCanDeserializeValidJson()
        {
            var jsonMessage = CreateValidJsonMessage(123, SensorType.Temperature, 25.5);

            var result = Deserializer.Deserialize(jsonMessage);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.SensorId, Is.EqualTo(123));
            Assert.That(result.Type, Is.EqualTo(SensorType.Temperature));
            Assert.That(result.Value, Is.EqualTo(25.5));
        }

        [Test]
        public void ThenCannotDeserializeInvalidJson()
        {
            var jsonMessage = CreateInvalidJsonMessage();

            Assert.Throws<InvalidOperationException>(() => Deserializer.Deserialize(jsonMessage));
        }
    }
}