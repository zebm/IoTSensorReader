using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Output
{
    /// <summary>
    /// Defines contract for outputing processed data. Implementations will determine output methods.
    /// </summary>
    public interface IOutputService
    {
        Task WriteAsync(SensorReading reading);
    }
}