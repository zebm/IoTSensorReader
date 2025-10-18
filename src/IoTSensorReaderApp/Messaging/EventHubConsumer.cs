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

            var partitionTasks = partitions.Select(partitionId => 
                ReadPartitionMessagesAsync(consumer, partitionId, cancellationToken)).ToArray();

            var channel = System.Threading.Channels.Channel.CreateUnbounded<string>();
            var writer = channel.Writer;

            var backgroundTasks = partitionTasks.Select(async partitionMessages =>
            {
                try
                {
                    await foreach (var message in partitionMessages)
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            await writer.WriteAsync(message, cancellationToken);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Partition task has been cancelled.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading from partition: {ex.Message}");
                }
            });

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.WhenAll(backgroundTasks);
                }
                finally
                {
                    writer.TryComplete();
                }
            }, cancellationToken);

            await foreach (var message in channel.Reader.ReadAllAsync(cancellationToken))
            {
                yield return message;
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