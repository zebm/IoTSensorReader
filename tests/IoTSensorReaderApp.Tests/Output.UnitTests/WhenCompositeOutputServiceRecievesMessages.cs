using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;
using NSubstitute;

namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class WhenCompositeOutputServiceRecievesMessages
    {
        private CompositeOutputService _compositeService;
        private IOutputService _mockService1;
        private IOutputService _mockService2;
        private SensorReading _sensorReading;

        [SetUp]
        public void SetUp()
        {
            _mockService1 = Substitute.For<IOutputService>();
            _mockService2 = Substitute.For<IOutputService>();
            var services = new List<IOutputService> { _mockService1, _mockService2 };
            _compositeService = new CompositeOutputService(services);

            _sensorReading = new SensorReading
            {
                SensorId = 123,
                Type = SensorType.Temperature,
                Value = 25.5,
                TimeStamp = DateTime.UtcNow
            };
        }

        [Test]
        public async Task ThenBothServicesReceiveTheReading()
        {
            await _compositeService.WriteAsync(_sensorReading);

            await _mockService1.Received(1).WriteAsync(_sensorReading);
            await _mockService2.Received(1).WriteAsync(_sensorReading);
        }

        [Test]
        public async Task ThenServicesAreCalledInOrder()
        {
            await _compositeService.WriteAsync(_sensorReading);

            Received.InOrder(async () =>
            {
                await _mockService1.WriteAsync(_sensorReading);
                await _mockService2.WriteAsync(_sensorReading);
            });
        }
    }
}