using System.Text.Json;
using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Formatting
{
    public class JsonSensorFormatter : ISensorFormatter
    {
        public String Format(SensorReading reading)
        {
            var json = JsonSerializer.Serialize(new
            {
                type = reading.Type.ToString(),
                value = reading.Value,
                timestamp = reading.TimeStamp,
                rawMessage = reading.RawMessage
            });

            return json;
        }
    }
}