using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Formatting
{
    public class TemperatureFormatter : ISensorFormatter
    {
        public string Format(SensorReading reading)
        {
            return $"Sensor {reading.SensorId} | [Temperature] {reading.TimeStamp}: {reading.Value}°C]";
        }
    }
}