using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Formatting
{
    public interface ISensorFormatter
    {
        bool CanFormat(SensorReading reading);
        string Format(SensorReading reading);
    }
}