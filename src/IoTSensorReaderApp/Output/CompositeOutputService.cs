using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Output
{
    public class CompositeOutputService : IOutputService
    {
        private readonly IEnumerable<IOutputService> _outputServices;

        public CompositeOutputService(IEnumerable<IOutputService> outputServices)
        {
            _outputServices = outputServices ?? throw new ArgumentNullException(nameof(outputServices));

            if (!_outputServices.Any())
            {
                throw new ArgumentException("At least one output service must be provided.", nameof(outputServices));
            }
        }

        public async Task WriteAsync(SensorReading reading)
        {
            foreach (var outputService in _outputServices)
            {
                await outputService.WriteAsync(reading);
            }
        }
    }
}