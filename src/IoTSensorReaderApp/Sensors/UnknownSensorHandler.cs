using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Sensors
{
    public class UnknownSensorHandler : ISensorReadingHandler
    {
        public bool CanHandle(SensorReading reading)
        {
            return true;
        }

        public void Handle(SensorReading reading)
        {
            // TODO: Implement handling logic for unknown readings i.e. alerting, additional processing, etc.
        }
    }
}