using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Sensors;
using IoTSensorReaderApp.Messaging;
using IoTSensorReaderApp.Processing;
using IoTSensorReaderApp.Configuration;
using IoTSensorReaderApp.Models;

class Program
{
    static async Task Main(string[] args)
    {
        IAppConfiguration config = new AppConfiguration();

        IOutputService outputService = new ConsoleOutputService();

        var sensorHandlers = new List<ISensorReadingHandler>
        {
            new TemperatureReadingHandler(outputService),
            new HumidityReadingHandler(outputService)
        };

        IMessageProcessor messageProcessor = new SensorMessageProcessor(sensorHandlers, outputService);

        IIoTHubListener listener = new IoTHubListener(messageProcessor, config);

        using var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cts.Cancel();
            Console.WriteLine("Cancelled!");
        };

        await listener.StartListeningAsync(cts.Token);
    }
}
