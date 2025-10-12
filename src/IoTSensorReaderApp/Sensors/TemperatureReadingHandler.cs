using System;
using System.Threading.Tasks;
using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;

namespace IoTSensorReaderApp.Sensors
{
    public class TemperatureReadingHandler : ISensorReadingHandler
    {
        private readonly IOutputService _output;

        public TemperatureReadingHandler(IOutputService output)
        {
            _output = output;
        }

        public bool CanHandle(SensorReading reading)
        {
            return reading.Type == SensorType.Temperature;
        }

        public Task HandleAsync(SensorReading reading)
        {
            return _output.WriteAsync($"Sensor {reading.SensorId} | [Temperature] {reading.TimeStamp}: {reading.Value}°C]");
        }
    }
}