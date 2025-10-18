using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

namespace IoTSensorReaderApp.E2ETests.Helpers
{
    public class DeviceSimulator : IDisposable
    {
        private readonly DeviceClient _deviceClient;
        private const int HardcodedDeviceId = 123456;

        public DeviceSimulator(string deviceConnectionString)
        {
            _deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
        }

        public async Task SendTemperatureReadingAsync(double temperature)
        {
            await SendMessageAsync(1, temperature);  
        }

        public async Task SendHumidityReadingAsync(double humidity)
        {
            await SendMessageAsync(2, humidity);    
        }

        private async Task SendMessageAsync(int type, double value)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffff");
            
            var messageData = new
            {
                SensorId = HardcodedDeviceId,
                Type = type,
                Value = value,
                TimeStamp = timestamp,
                RawMessage = JsonConvert.SerializeObject(new
                {
                    SensorId = HardcodedDeviceId,
                    Type = type,
                    Value = value,
                    TimeStamp = timestamp
                })
            };

            var messageJson = JsonConvert.SerializeObject(messageData);
            var message = new Message(Encoding.UTF8.GetBytes(messageJson));

            await _deviceClient.SendEventAsync(message);
        }

        public void Dispose()
        {
            _deviceClient?.Dispose();
        }
    }
}