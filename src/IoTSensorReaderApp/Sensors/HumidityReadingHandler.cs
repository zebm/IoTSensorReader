using System;
using System.Threading.Tasks;
using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;

namespace IoTSensorReaderApp.Sensors
{
    public class HumidityReadingHandler : ISensorReadingHandler
    {
        private readonly IOutputService _output;

        public HumidityReadingHandler(IOutputService output)
        {
            _output = output;
        }

        public bool CanHandle(SensorReading reading)
        {
            return reading.Type == SensorType.Humidity;
        }

        public Task HandleAsync(SensorReading reading)
        {
            return _output.WriteAsync($"Sensor {reading.SensorId} | [Humidity] {reading.TimeStamp}: {reading.Value}%]");
        }
    }
}