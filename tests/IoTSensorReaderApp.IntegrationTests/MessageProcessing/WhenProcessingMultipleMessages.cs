using NUnit.Framework;
using System.Threading.Tasks;

namespace IoTSensorReaderApp.IntegrationTests.MessageProcessing
{
    [TestFixture]
    public class WhenProcessingMultipleMessages : MessageProcessingTest
    {
        [Test]
        public async Task ThenBothSensorTypesProcessCorrectly()
        {
            var tempReading = CreateTemperatureReading(111, 18.0);
            var humidityReading = CreateHumidityReading(222, 60.0);

            await Processor.ProcessMessageAsync(tempReading);
            await Processor.ProcessMessageAsync(humidityReading);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("Temperature"));
            Assert.That(output, Does.Contain("18°C"));
            Assert.That(output, Does.Contain("Humidity"));
            Assert.That(output, Does.Contain("60%"));
        }

        [Test]
        public async Task ThenMultipleTemperatureReadingsProcessInOrder()
        {
            var reading1 = CreateTemperatureReading(333, 15.0);
            var reading2 = CreateTemperatureReading(444, 25.0);

            await Processor.ProcessMessageAsync(reading1);
            await Processor.ProcessMessageAsync(reading2);

            var output = GetConsoleOutput();
            var firstIndex = output.IndexOf("333");
            var secondIndex = output.IndexOf("444");
            Assert.That(firstIndex, Is.LessThan(secondIndex));
        }

        [Test]
        public async Task ThenProcessesJsonMessagesSequentially()
        {
            var json1 = CreateValidJsonMessage(555, 1, 10.0);
            var json2 = CreateValidJsonMessage(666, 2, 80.0);

            var reading1 = Deserializer.Deserialize(json1);
            var reading2 = Deserializer.Deserialize(json2);
            
            await Processor.ProcessMessageAsync(reading1);
            await Processor.ProcessMessageAsync(reading2);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("555"));
            Assert.That(output, Does.Contain("10°C"));
            Assert.That(output, Does.Contain("666"));
            Assert.That(output, Does.Contain("80%"));
        }
    }
}