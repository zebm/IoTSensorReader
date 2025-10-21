using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Formatting
{
    public class UnknownSensorFormatter : ISensorFormatter
    {
        public bool CanFormat(SensorReading reading)
        {
            return reading.Type == SensorType.Unknown;
        }

        public string Format(SensorReading reading)
        {
            return $"Sensor {reading.SensorId} | [Unknown Sensor Type] {reading.TimeStamp}: {reading.Value} (Raw: {reading.RawMessage})";
        }
    }
}