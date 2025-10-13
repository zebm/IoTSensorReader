namespace IoTSensorReaderApp.Messaging
{
    /// <summary>
    /// Coordinates messages from a messaging system and delegates deserialisation and processing.
    /// </summary>
    public interface IMessageCoordinator
    {
        /// <summary>
        /// Listens for incoming messages 
        /// </summary>
        Task StartListeningAsync(CancellationToken cancellationToken);

    }
}