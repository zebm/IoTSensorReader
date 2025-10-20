using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Formatting
{
    public class HumidityFormatter : ISensorFormatter
    {
        public bool CanFormat(SensorReading reading)
        {
            return reading.Type == SensorType.Humidity;
        }

        public string Format(SensorReading reading)
        {
            return $"Sensor {reading.SensorId} | [Humidity] {reading.TimeStamp}: {reading.Value}%]";
        }
    }
}