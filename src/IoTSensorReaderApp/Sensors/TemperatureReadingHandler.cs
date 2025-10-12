using System;
using System.Threading.Tasks;
using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Formatting;


namespace IoTSensorReaderApp.Sensors
{
    public class TemperatureReadingHandler : ISensorReadingHandler
    {
        private readonly IOutputService _output;
        private readonly ISensorFormatter _formatter;

        public TemperatureReadingHandler(IOutputService output, ISensorFormatter formatter)
        {
            _output = output;
            _formatter = formatter;
        }

        public bool CanHandle(SensorReading reading)
        {
            return reading.Type == SensorType.Temperature;
        }

        public Task HandleAsync(SensorReading reading)
        {
            var formatted = _formatter.Format(reading);
            return _output.WriteAsync(formatted);
        }
    }
}