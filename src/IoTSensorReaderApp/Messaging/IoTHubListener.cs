using System;
using System.Text;
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
        private readonly IMessageProcesser _processor;
        private readonly IAppConfiguration _config;

        public IoTHubListener(IMessageProcessor processor, IAppConfiguration config)
        {
            _processor = processor;
            _config = config;
        }

        public async Task StartListeningAsync(CancellationToken cancellationToken)
        {
            var connectionString = _config.IoTHubConnectionString;
            var consumerGroup = EventHubCosumerClient.DefaultConsumerGroupName;

            await using var consumer = new EventHubConsumerClient(consumerGroup, connectionString);

            await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(cancellationToken))
            {
                var messageBody = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());

                var reading = new SensorReading
                {
                    RawMessage = messageBody,
                    TimestampAttribute = DateTime.UtcNow
                };

                await _processor.ProcessMessageAsync(reading);
            }
        }
    }
}