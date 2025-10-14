using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Formatting;

namespace IoTSensorReaderApp.Sensors
{
    public class HumidityReadingHandler : ISensorReadingHandler
    {
        private readonly IOutputService _output;
        private readonly ISensorFormatter _formatter;

        public HumidityReadingHandler(IOutputService output, ISensorFormatter formatter)
        {
            _output = output;
            _formatter = formatter;
        }

        public bool CanHandle(SensorReading reading)
        {
            return reading.Type == SensorType.Humidity;
        }

        public Task HandleAsync(SensorReading reading)
        {
            var formatted = _formatter.Format(reading);
            return _output.WriteAsync(formatted);
        }
    }
}