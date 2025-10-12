using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs.Consumer;
using IoTSensorReaderApp.Configuration;
using IoTSensorReaderApp.Processing;
using IoTSensorReaderApp.Models;
using System.ComponentModel.DataAnnotations;

namespace IoTSensorReaderApp.Messaging
{
    /// <summary>
    /// Listens for messages from Azure IoT Hub via the Event Hub endpoint.
    /// Will parse incoming messages and sends to message processer
    /// </summary>
    public class IoTHubListener : IIoTHubListener
    {
        private readonly IMessageProcessor _processor;
        private readonly IAppConfiguration _config;

        public IoTHubListener(IMessageProcessor processor, IAppConfiguration config)
        {
            _processor = processor;
            _config = config;
        }

        public async Task StartListeningAsync(CancellationToken cancellationToken)
        {
            var connectionString = _config.IoTHubConnectionString;
            var consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            await using var consumer = new EventHubConsumerClient(consumerGroup, connectionString);

            var partitions = await consumer.GetPartitionIdsAsync();
            var tasks = new List<Task>();

            foreach (var partitionId in partitions)
            {

                var task = Task.Run(async () =>
                {
                    try
                    {
                        await foreach (PartitionEvent partitionEvent in consumer.ReadEventsFromPartitionAsync(
                            partitionId,
                            EventPosition.Latest,
                            cancellationToken))
                        {
                            var messageBody = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());

                            SensorReading reading;

                            try
                            {
                                reading = JsonSerializer.Deserialize<SensorReading>(messageBody);
                                reading.RawMessage = messageBody;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Deserialization failed: {ex.Message}");
                                reading = new SensorReading
                                {
                                    SensorId = 0,
                                    Type = SensorType.Unknown,
                                    Value = null,
                                    TimeStamp = DateTime.UtcNow,
                                    RawMessage = messageBody
                                };
                            }

                            await _processor.ProcessMessageAsync(reading);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in partition {partitionId}: {ex.Message}");
                    }
                }, cancellationToken);

                tasks.Add(task);
            }

            await Task.WhenAll(tasks); 
        }
    }
}