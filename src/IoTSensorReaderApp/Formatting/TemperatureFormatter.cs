using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Formatting
{
    public class TemperatureFormatter : ISensorFormatter
    {
        public bool CanFormat(SensorReading reading)
        {
            return reading.Type == SensorType.Temperature;
        }

        public string Format(SensorReading reading)
        {
            return $"Sensor {reading.SensorId} | [Temperature] {reading.TimeStamp}: {reading.Value}°C]";
        }
    }
}