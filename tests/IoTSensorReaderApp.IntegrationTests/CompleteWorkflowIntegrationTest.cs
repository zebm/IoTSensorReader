using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Messaging;
using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Processing;
using IoTSensorReaderApp.Sensors;

namespace IoTSensorReaderApp.IntegrationTests
{
    [TestFixture]
    public class CompleteWorkflowIntegrationTest : IntegrationTest
    {
        [Test]
        public async Task WhenProcessingCompleteWorkflowThenAllComponentsWorkTogether()
        {
            var deserializer = new JsonMessageDeserializer();
            var formatters = new List<ISensorFormatter>
            {
                new TemperatureFormatter(),
                new HumidityFormatter()
            };
            var jsonFormatter = new JsonSensorFormatter();
            var consoleService = new ConsoleOutputService(formatters);
            var tempFormatter = new TemperatureFormatter();
            var tempHandler = new TemperatureReadingHandler();
            var handlers = new List<ISensorReadingHandler> { tempHandler };
            var processor = new SensorMessageProcessor(handlers, consoleService);
            
            var jsonMessage = """
            {
                "SensorId": 99999,
                "Type": 1,
                "Value": 23.5,
                "TimeStamp": "2025-10-17T14:30:00"
            }
            """;

            var sensorReading = deserializer.Deserialize(jsonMessage);
            await processor.ProcessMessageAsync(sensorReading);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("Temperature"));
            Assert.That(output, Does.Contain("23.5°C"));
            Assert.That(output, Does.Contain("99999"));
        }

        [Test]
        public async Task WhenProcessingMultipleSensorTypesInWorkflowThenAllProcessCorrectly()
        {
            var deserializer = new JsonMessageDeserializer();
            var formatters = new List<ISensorFormatter>
            {
                new TemperatureFormatter(),
                new HumidityFormatter()
            };

            var jsonFormatter = new JsonSensorFormatter();
            var consoleService = new ConsoleOutputService(formatters);
            var compositeService = new CompositeOutputService(new List<IOutputService> { consoleService });
            
            var tempHandler = new TemperatureReadingHandler();
            var humidityHandler = new HumidityReadingHandler();
            var handlers = new List<ISensorReadingHandler> { tempHandler, humidityHandler };
            
            var processor = new SensorMessageProcessor(handlers, compositeService);
            
            var tempJson = """{"SensorId": 1001, "Type": 1, "Value": 19.5, "TimeStamp": "2025-10-17T09:00:00"}""";
            var humidityJson = """{"SensorId": 1002, "Type": 2, "Value": 55.0, "TimeStamp": "2025-10-17T09:01:00"}""";

            var tempReading = deserializer.Deserialize(tempJson);
            var humidityReading = deserializer.Deserialize(humidityJson);
            
            await processor.ProcessMessageAsync(tempReading);
            await processor.ProcessMessageAsync(humidityReading);

            var output = GetConsoleOutput();
            Assert.That(output, Does.Contain("1001"));
            Assert.That(output, Does.Contain("19.5°C"));
            Assert.That(output, Does.Contain("1002"));
            Assert.That(output, Does.Contain("55%"));
        }
    }
}