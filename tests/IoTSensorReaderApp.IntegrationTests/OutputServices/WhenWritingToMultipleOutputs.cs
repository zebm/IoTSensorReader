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

            await compositeService.WriteAsync(reading);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("555"));
            Assert.That(output, Does.Contain("30"));
            Assert.That(output, Does.Contain("°C"));
        }

        [Test]
        public async Task ThenDifferentFormattersProduceDifferentOutputForTemperature()
        {
            var reading = new SensorReading
            {
                SensorId = 666,
                Type = SensorType.Temperature,
                Value = 35.0,
                TimeStamp = DateTime.Now
            };

            await ConsoleService.WriteAsync(reading);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("666"));
            Assert.That(output, Does.Contain("35"));
            Assert.That(output, Does.Contain("°C"));
            Assert.That(output, Does.Contain("Temperature"));
        }

        [Test]
        public async Task ThenMultipleReadingsWithCompositeService()
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
            
            await compositeService.WriteAsync(tempReading);
            await compositeService.WriteAsync(humidityReading);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("777"));
            Assert.That(output, Does.Contain("28"));
            Assert.That(output, Does.Contain("°C"));
            Assert.That(output, Does.Contain("888"));
            Assert.That(output, Does.Contain("70"));
            Assert.That(output, Does.Contain("%"));
        }
    }
}