using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Tests.Messaging.UnitTests
{
    public class MessagingTest
    {
        protected string CreateValidJsonMessage(int sensorId, SensorType type, double value)
        {
            return $$"""
            {
                "SensorId": {{sensorId}},
                "Type": {{(int)type}},
                "Value": {{value}},
                "TimeStamp": "2025-10-17T10:30:00"
            }
            """;
        }

        protected string CreateInvalidJsonMessage()
        {
            return "{ this is not valid JSON }";
        }

        protected SensorReading CreateExpectedSensorReading(int sensorId = 12345, SensorType type = SensorType.Temperature, double value = 25.5)
        {
            return new SensorReading
            {
                SensorId = sensorId,
                Type = type,
                Value = value,
                TimeStamp = DateTime.Parse("2025-10-17T10:30:00")
            };
        }
    }
}