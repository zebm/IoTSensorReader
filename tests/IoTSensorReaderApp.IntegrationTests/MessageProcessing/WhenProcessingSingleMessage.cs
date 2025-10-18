using NUnit.Framework;
using System.Threading.Tasks;

namespace IoTSensorReaderApp.IntegrationTests.MessageProcessing
{
    [TestFixture]
    public class WhenProcessingSingleMessage : MessageProcessingTest
    {
        [Test]
        public async Task ThenTemperatureMessageOutputsCorrectly()
        {
            var temperatureReading = CreateTemperatureReading(12345, 22.5);

            await Processor.ProcessMessageAsync(temperatureReading);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("Temperature"));
            Assert.That(output, Does.Contain("22.5°C"));
            Assert.That(output, Does.Contain("12345"));
        }

        [Test]
        public async Task ThenHumidityMessageOutputsCorrectly()
        {
            var humidityReading = CreateHumidityReading(67890, 75.0);

            await Processor.ProcessMessageAsync(humidityReading);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("Humidity"));
            Assert.That(output, Does.Contain("75%"));
            Assert.That(output, Does.Contain("67890"));
        }

        [Test]
        public async Task ThenJsonDeserializationWorksWithProcessing()
        {
            var jsonMessage = CreateValidJsonMessage(11111, 1, 20.0);

            var reading = Deserializer.Deserialize(jsonMessage);
            await Processor.ProcessMessageAsync(reading);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("11111"));
            Assert.That(output, Does.Contain("20°C"));
        }
    }
}