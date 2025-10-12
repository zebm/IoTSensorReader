using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Formatting
{
    public interface ISensorFormatter
    {
        string Format(SensorReading reading);
    }
}