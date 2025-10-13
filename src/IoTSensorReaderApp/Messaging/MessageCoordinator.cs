using IoTSensorReaderApp.Processing;

namespace IoTSensorReaderApp.Messaging
{
    public class MessageCoordinator : IMessageCoordinator
    {
        private readonly IEventHubConsumer _eventHubConsumer;
        private readonly IMessageProcessor _processor;
        private readonly IMessageDeserializer _deserializer;

        public MessageCoordinator(
            IEventHubConsumer eventHubConsumer,
            IMessageProcessor processor,
            IMessageDeserializer deserializer)
        {
            _eventHubConsumer = eventHubConsumer;
            _processor = processor;
            _deserializer = deserializer;
        }

        public async Task StartListeningAsync(CancellationToken cancellationToken)
        {
            await foreach (var messageBody in _eventHubConsumer.ReadMessagesAsync(cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Cancellation requested.");
                    break;
                }

                try
                {
                    var reading = _deserializer.Deserialize(messageBody);
                    await _processor.ProcessMessageAsync(reading);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            }
        }
    }
}