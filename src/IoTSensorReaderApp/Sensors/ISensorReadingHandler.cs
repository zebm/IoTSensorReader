using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Sensors
{
    public interface ISensorReadingHandler
    {
        bool CanHandle(SensorReading reading);

        void Handle(SensorReading reading);
    }
}