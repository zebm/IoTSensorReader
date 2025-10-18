using System.Text;

namespace IoTSensorReaderApp.E2ETests.Helpers
{
    public class ConsoleOutputCapture : IDisposable
    {
        private readonly TextWriter _originalOut;
        private readonly StringWriter _stringWriter;
        private readonly StringBuilder _stringBuilder;

        public ConsoleOutputCapture()
        {
            _originalOut = Console.Out;
            _stringBuilder = new StringBuilder();
            _stringWriter = new StringWriter(_stringBuilder);
            Console.SetOut(_stringWriter);
        }

        public string GetOutput()
        {
            _stringWriter.Flush();
            return _stringBuilder.ToString();
        }

        public List<string> GetOutputLines()
        {
            var output = GetOutput();
            return output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public bool ContainsTemperatureReading(double expectedValue)
        {
            var output = GetOutput();
            return output.Contains($"Temperature: {expectedValue}") || 
                   output.Contains($"{expectedValue}°C") ||
                   output.Contains($"{expectedValue} C");
        }

        public bool ContainsHumidityReading(double expectedValue)
        {
            var output = GetOutput();
            return output.Contains($"Humidity: {expectedValue}") || 
                   output.Contains($"{expectedValue}%");
        }

        public bool ContainsDeviceId(string deviceId)
        {
            var output = GetOutput();
            return output.Contains(deviceId);
        }



        public void Clear()
        {
            _stringBuilder.Clear();
        }

        public void Dispose()
        {
            Console.SetOut(_originalOut);
            _stringWriter?.Dispose();
        }
    }
}