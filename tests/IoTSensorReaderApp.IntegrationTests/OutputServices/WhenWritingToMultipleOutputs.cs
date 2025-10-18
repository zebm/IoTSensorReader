using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;

namespace IoTSensorReaderApp.IntegrationTests.OutputServices
{
    [TestFixture]
    public class WhenWritingToMultipleOutputs : OutputIntegrationTest
    {
        [Test]
        public async Task ThenCompositeServiceWritesToConsole()
        {
            var services = new List<IOutputService> { ConsoleService };
            var compositeService = new CompositeOutputService(services);
            var reading = new SensorReading
            {
                SensorId = 555,
                Type = SensorType.Temperature,
                Value = 30.0,
                TimeStamp = DateTime.Now
            };

            var formattedMessage = TemperatureFormatter.Format(reading);
            await compositeService.WriteAsync(formattedMessage);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("555"));
            Assert.That(output, Does.Contain("30°C"));
        }

        [Test]
        public async Task ThenDifferentFormattersProduceDifferentOutput()
        {
            var reading = new SensorReading
            {
                SensorId = 666,
                Type = SensorType.Temperature,
                Value = 35.0,
                TimeStamp = DateTime.Now
            };

            var humanReadable = TemperatureFormatter.Format(reading);
            var jsonFormat = JsonFormatter.Format(reading);
            
            await ConsoleService.WriteAsync("Human: " + humanReadable);
            await ConsoleService.WriteAsync("JSON: " + jsonFormat);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("35°C"));
            Assert.That(output, Does.Contain("\"type\":"));
        }

        [Test]
        public async Task ThenMultipleFormattersWithCompositeService()
        {
            var services = new List<IOutputService> { ConsoleService };
            var compositeService = new CompositeOutputService(services);
            
            var tempReading = new SensorReading
            {
                SensorId = 777,
                Type = SensorType.Temperature,
                Value = 28.0,
                TimeStamp = DateTime.Now
            };

            var humidityReading = new SensorReading
            {
                SensorId = 888,
                Type = SensorType.Humidity,
                Value = 70.0,
                TimeStamp = DateTime.Now
            };

            var tempFormatted = TemperatureFormatter.Format(tempReading);
            var humidityFormatted = HumidityFormatter.Format(humidityReading);
            
            await compositeService.WriteAsync(tempFormatted);
            await compositeService.WriteAsync(humidityFormatted);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("777"));
            Assert.That(output, Does.Contain("28°C"));
            Assert.That(output, Does.Contain("888"));
            Assert.That(output, Does.Contain("70%"));
        }
    }
}