using System.Threading.Tasks;
using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Processing
{
    /// <summary>
    /// Defines the contract for processing sensor messages.
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// Process a sensor message reacieved from IoT Sensor device.
        /// </summary>
        /// <param name="reading">The sensor reading to be processed.</param>
        Task ProcessMessageAsync(SensorReading reading);
    }
}