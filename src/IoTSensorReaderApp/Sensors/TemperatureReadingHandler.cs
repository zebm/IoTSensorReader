using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Formatting;

namespace IoTSensorReaderApp.Sensors
{
    public class TemperatureReadingHandler : ISensorReadingHandler
    {
        public bool CanHandle(SensorReading reading)
        {
            return reading.Type == SensorType.Temperature;
        }

        public void Handle(SensorReading reading)
        {
            // TODO: Implement handling logic for temperature readings i.e. alerting, additional processing, etc.
        }
    }
}