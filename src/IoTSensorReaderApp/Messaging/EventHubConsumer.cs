using System.Text;
using Azure.Messaging.EventHubs.Consumer;
using IoTSensorReaderApp.Configuration;
using System.Runtime.CompilerServices;

namespace IoTSensorReaderApp.Messaging
{
    public class EventHubConsumer : IEventHubConsumer
    {
        private readonly IAppConfiguration _config;

        public EventHubConsumer(IAppConfiguration config)
        {
            _config = config;
        }

        public async IAsyncEnumerable<string> ReadMessagesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var connectionString = _config.IoTHubConnectionString;
            var consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            await using var consumer = new EventHubConsumerClient(consumerGroup, connectionString);
            var partitions = await consumer.GetPartitionIdsAsync();

            foreach (var partitionId in partitions)
            {
                await foreach (var message in ReadPartitionMessagesAsync(consumer, partitionId, cancellationToken))
                {
                    yield return message;
                }
            }
        }

        private async IAsyncEnumerable<string> ReadPartitionMessagesAsync(EventHubConsumerClient consumer, string partitionId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var partitionEvent in consumer.ReadEventsFromPartitionAsync(partitionId, EventPosition.Latest, cancellationToken))
            {
                yield return Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
            }
        }
    }
}