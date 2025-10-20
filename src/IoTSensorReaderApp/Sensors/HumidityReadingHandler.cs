using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Formatting;

namespace IoTSensorReaderApp.Sensors
{
    public class HumidityReadingHandler : ISensorReadingHandler
    {
        public bool CanHandle(SensorReading reading)
        {
            return reading.Type == SensorType.Humidity;
        }

        public void Handle(SensorReading reading)
        {
            // TODO: Implement handling logic for humidity readings i.e. alerting, additional processing, etc.
        }
    }
}