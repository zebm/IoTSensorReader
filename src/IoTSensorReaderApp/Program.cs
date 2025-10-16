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
        IDbConfiguration dbConfig = new DbConfiguration();

        var consoleOutputService = new ConsoleOutputService();
        var dbOutputService = new DbOutputService(dbConfig);

        var compositeOutputService = new CompositeOutputService(new List<IOutputService> { consoleOutputService, dbOutputService });

        var sensorHandlers = new List<ISensorReadingHandler>
        {
            new TemperatureReadingHandler(consoleOutputService, new TemperatureFormatter()),
            new HumidityReadingHandler(consoleOutputService, new HumidityFormatter()),
            new JsonHandler(dbOutputService, new JsonSensorFormatter())

        };

        IMessageProcessor messageProcessor = new SensorMessageProcessor(sensorHandlers, compositeOutputService);

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
