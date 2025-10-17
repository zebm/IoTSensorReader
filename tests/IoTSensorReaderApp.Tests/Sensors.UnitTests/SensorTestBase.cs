using NSubstitute;
using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Formatting;

namespace IoTSensorReaderApp.Tests.Sensors.UnitTests
{
    public class SensorTest
    {
        protected IOutputService MockOutputService;
        protected ISensorFormatter MockFormatter;

        [SetUp]
        public void SetUp()
        {
            MockOutputService = Substitute.For<IOutputService>();
            MockFormatter = Substitute.For<ISensorFormatter>();
        }

        protected SensorReading CreateTemperatureReading(double value = 22.5)
        {
            return new SensorReading
            {
                SensorId = 123,
                Type = SensorType.Temperature,
                Value = value,
                TimeStamp = DateTime.Now
            };
        }

        protected SensorReading CreateHumidityReading(double value = 65.0)
        {
            return new SensorReading
            {
                SensorId = 456,
                Type = SensorType.Humidity,
                Value = value,
                TimeStamp = DateTime.Now
            };
        }
    }
}