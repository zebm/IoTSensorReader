using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Formatting
{
    public class HumidityFormatter : ISensorFormatter
    {
        public string Format(SensorReading reading)
        {
            return $"Sensor {reading.SensorId} | [Humidity] {reading.TimeStamp}: {reading.Value}%]";
        }
    }
}