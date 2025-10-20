using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Messaging;
using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Processing;
using IoTSensorReaderApp.Sensors;

namespace IoTSensorReaderApp.IntegrationTests.MessageProcessing
{
    [TestFixture]
    public class MessageProcessingTest : IntegrationTest
    {
        protected JsonMessageDeserializer Deserializer;
        protected ConsoleOutputService ConsoleService;
        protected SensorMessageProcessor Processor;
        protected List<ISensorReadingHandler> Handlers;

        [SetUp]
        public void SetUp()
        {
            BaseSetUp();

            Deserializer = new JsonMessageDeserializer();
            var formatters = new List<ISensorFormatter>
            {
                new TemperatureFormatter(),
                new HumidityFormatter()
            };        

            ConsoleService = new ConsoleOutputService(formatters);
            
            var tempHandler = new TemperatureReadingHandler();
            var humidityHandler = new HumidityReadingHandler();
            
            Handlers = new List<ISensorReadingHandler> { tempHandler, humidityHandler };
            Processor = new SensorMessageProcessor(Handlers, ConsoleService);
        }

        protected SensorReading CreateTemperatureReading(int sensorId = 12345, double value = 25.5)
        {
            return new SensorReading
            {
                SensorId = sensorId,
                Type = SensorType.Temperature,
                Value = value,
                TimeStamp = DateTime.Parse("2025-10-17T10:30:00")
            };
        }

        protected SensorReading CreateHumidityReading(int sensorId = 67890, double value = 65.0)
        {
            return new SensorReading
            {
                SensorId = sensorId,
                Type = SensorType.Humidity,
                Value = value,
                TimeStamp = DateTime.Parse("2025-10-17T10:30:00")
            };
        }

        protected string CreateValidJsonMessage(int sensorId = 12345, int sensorType = 1, double value = 25.5)
        {
            return $$"""
            {
                "SensorId": {{sensorId}},
                "Type": {{sensorType}},
                "Value": {{value}},
                "TimeStamp": "2025-10-17T10:30:00"
            }
            """;
        }
    }
}