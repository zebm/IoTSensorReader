using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Sensors;

namespace IoTSensorReaderApp.Processing
{
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
            var handler = _handlers.FirstOrDefault(h => h.CanHandle(reading));

            if (handler != null)
            {
                handler.Handle(reading);
                await _output.WriteAsync(reading);
            }
            else
            {
                throw new InvalidOperationException($"No handler found for sensor type: {reading.Type}");
            }
        }
    }
}