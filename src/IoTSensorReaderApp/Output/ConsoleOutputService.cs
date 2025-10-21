using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Output
{
    public class ConsoleOutputService : IOutputService
    {
        private  IEnumerable<ISensorFormatter> _formatters;

        public ConsoleOutputService(IEnumerable<ISensorFormatter> formatters)
        {
            _formatters = formatters ?? throw new ArgumentNullException(nameof(formatters));

            if (!_formatters.Any()) 
            {
                throw new ArgumentException("At least one formatter must be provided.", nameof(formatters));
            }
        }
        
        public Task WriteAsync(SensorReading reading)
        {
            var formatter = _formatters.FirstOrDefault(f => f.CanFormat(reading));
            if (formatter != null)
            {
                var formattedMessage = formatter.Format(reading);
                Console.WriteLine(formattedMessage);
                return Task.CompletedTask;
            }else
            {
                throw new InvalidOperationException($"No handler found for sensor type: {reading.Type}");
            }
        }
    }
}               
        