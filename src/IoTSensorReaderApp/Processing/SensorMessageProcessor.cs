using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Sensors;

namespace IoTSensorReaderApp.Processing
{
    /// <summary>
    /// Sends sensor reading to the correct handler based on the sensor type.
    /// </summary>
    public class SensorMessageProcessor : IMessageProcessor
    {
        private readonly IEnumerable<ISensorReadingHandler> _handlers;
        private readonly IOutputService _output;

        public SensorMessageProcessor(IEnumerable<ISensorReadingHandler> handlers, IOutputService output)
        {
            _handlers = handlers;
            _output = output;
        }

        public async Task ProcessMessageAsync(SensorReading reading)
        {
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(reading))
                {
                    await handler.HandleAsync(reading);
                    return;
                }
            }

            await _output.WriteAsync($"No handler found for sensor type: {reading.Type}");
        }
    }
}