using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Messaging
{
    public interface IMessageDeserializer
    {
        SensorReading Deserialize(string messageBody);
    }
}