using System.Threading.Tasks;
using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Sensors
{
    /// <summary>
    /// Defines contract for handling a type of sensor data. Implementations to define specific type of readings.
    /// </summary>
    public interface ISensorReadingHandler
    {
        bool CanHandle(SensorReading reading);

        Task HandleAsync(SensorReading reading);
    }
}