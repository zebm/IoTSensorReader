using System.Text.Json;
using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Messaging
{
    public class JsonMessageDeserializer : IMessageDeserializer
    {
        public SensorReading Deserialize(string messageBody)
        {
            try
            {
                var reading = JsonSerializer.Deserialize<SensorReading>(messageBody);
                if (reading != null)
                {
                    reading.RawMessage = messageBody;
                    return reading;
                }

                throw new InvalidOperationException("Deserialized object is null.");
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Failed to deserialize message: {ex.Message}", ex);
            }
        }
    }
}