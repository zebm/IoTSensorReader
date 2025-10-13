using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Sensors;
using IoTSensorReaderApp.Messaging;
using IoTSensorReaderApp.Processing;
using IoTSensorReaderApp.Configuration;
using IoTSensorReaderApp.Formatting;

class Program
{
    static async Task Main(string[] args)
    {
        IAppConfiguration config = new AppConfiguration();

        IOutputService outputService = new ConsoleOutputService();

        var sensorHandlers = new List<ISensorReadingHandler>
        {
            new TemperatureReadingHandler(outputService, new TemperatureFormatter()),
            new HumidityReadingHandler(outputService, new HumidityFormatter())
        };

        IMessageProcessor messageProcessor = new SensorMessageProcessor(sensorHandlers, outputService);

        IEventHubConsumer eventHubConsumer = new EventHubConsumer(config);
        IMessageDeserializer deserializer = new JsonMessageDeserializer();

        IMessageCoordinator listener = new MessageCoordinator(eventHubConsumer, messageProcessor, deserializer);

        using var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cts.Cancel();
        };

        try
        {
            await listener.StartListeningAsync(cts.Token);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Operation canceled.");
        }
    }
}
