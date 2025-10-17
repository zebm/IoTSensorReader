using NSubstitute;
using IoTSensorReaderApp.Processing;
using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Sensors;
using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Tests.Processing.UnitTests
{
    public class SensorMessageProcessorTest
    {
        protected IOutputService MockOutputService { get; private set; } = null!;
        protected ISensorReadingHandler MockTemperatureHandler { get; private set; } = null!;
        protected ISensorReadingHandler MockHumidityHandler { get; private set; } = null!;
        protected List<ISensorReadingHandler> Handlers { get; private set; } = null!;
        protected SensorMessageProcessor SystemUnderTest { get; private set; } = null!;

        [SetUp]
        public virtual void SetUp()
        {
            MockOutputService = Substitute.For<IOutputService>();
            MockTemperatureHandler = Substitute.For<ISensorReadingHandler>();
            MockHumidityHandler = Substitute.For<ISensorReadingHandler>();

            Handlers = new List<ISensorReadingHandler> 
            { 
                MockTemperatureHandler, 
                MockHumidityHandler 
            };

            SystemUnderTest = new SensorMessageProcessor(Handlers, MockOutputService);
        }

        protected static SensorReading CreateTemperatureReading(int sensorId = 123, double value = 22.5)
        {
            return new SensorReading
            {
                SensorId = sensorId,
                Type = SensorType.Temperature,
                Value = value,
                TimeStamp = DateTime.Now,
                RawMessage = "test message"
            };
        }

        protected static SensorReading CreateHumidityReading(int sensorId = 456, double value = 65.0)
        {
            return new SensorReading
            {
                SensorId = sensorId,
                Type = SensorType.Humidity,
                Value = value,
                TimeStamp = DateTime.Now,
                RawMessage = "test message"
            };
        }

        protected void SetupTemperatureHandler()
        {
            MockTemperatureHandler
                .CanHandle(Arg.Is<SensorReading>(r => r.Type == SensorType.Temperature))
                .Returns(true);
            
            MockTemperatureHandler
                .CanHandle(Arg.Is<SensorReading>(r => r.Type != SensorType.Temperature))
                .Returns(false);
        }

        protected void SetupHumidityHandler()
        {
            MockHumidityHandler
                .CanHandle(Arg.Is<SensorReading>(r => r.Type == SensorType.Humidity))
                .Returns(true);
            
            MockHumidityHandler
                .CanHandle(Arg.Is<SensorReading>(r => r.Type != SensorType.Humidity))
                .Returns(false);
        }
    }
}