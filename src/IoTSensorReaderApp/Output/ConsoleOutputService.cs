using System;
using System.Threading.Tasks;

namespace IoTSensorReaderApp.Output
{
    public class ConsoleOutputService : IOutputService
    {
        public Task WriteAsync(string message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}