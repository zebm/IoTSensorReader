using System.Threading;
using System.Threading.Tasks;

namespace IoTSensorReaderApp.Messaging
{
    /// <summary>
    /// Define contract for service to listen for messages from a IoT HUB
    /// </summary>
    public interface IIoTHubListener
    {
        /// <summary>
        /// Listens for incoming messages 
        /// </summary>
        Task StartListeningAsync(CancellationToken cancellationToken);

    }
}