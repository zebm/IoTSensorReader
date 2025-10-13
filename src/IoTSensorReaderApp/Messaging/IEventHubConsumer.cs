namespace IoTSensorReaderApp.Messaging
{
    public interface IEventHubConsumer
    {
        IAsyncEnumerable<string> ReadMessagesAsync(CancellationToken cancellationToken);
    }
}