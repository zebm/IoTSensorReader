using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoTSensorReaderApp.Output
{
    public class CompositeOutputService : IOutputService
    {
        private readonly IEnumerable<IOutputService> _outputServices;

        public CompositeOutputService(IEnumerable<IOutputService> outputServices)
        {
            _outputServices = outputServices ?? throw new ArgumentNullException(nameof(outputServices));
        }

        public async Task WriteAsync(string message)
        {
            foreach (var outputService in _outputServices)
            {
                await outputService.WriteAsync(message);
            }
        }
    }
}