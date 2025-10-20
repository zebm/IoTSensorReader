using IoTSensorReaderApp.Models;
using NSubstitute;

namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class WhenWritingToConsole : ConsoleOutputServiceTest
    {
        private SensorReading _sensorReading;

        [SetUp]
        public new void SetUp()
        {
            base.TestSetUp();
            _sensorReading = new SensorReading
            {
                SensorId = 123,
                Type = SensorType.Temperature,
                Value = 22.5,
                TimeStamp = DateTime.UtcNow
            };
        }

        [Test]
        public async Task ThenFormatterIsCalledAndOutputWrittenToConsole()
        {
            var expectedMessage = "Sensor 123 reading: 22.5";
            MockFormatter.CanFormat(_sensorReading).Returns(true);
            MockFormatter.Format(_sensorReading).Returns(expectedMessage);

            await OutputService.WriteAsync(_sensorReading);

            var output = GetCapturedOutput();
            Assert.That(output, Does.Contain(expectedMessage));
        }

        [Test]
        public async Task ThenFormatterCanFormatIsCheckedFirst()
        {
            MockFormatter.CanFormat(_sensorReading).Returns(true);
            MockFormatter.Format(_sensorReading).Returns("formatted");

            await OutputService.WriteAsync(_sensorReading);

            Received.InOrder(() =>
            {
                MockFormatter.CanFormat(_sensorReading);
                MockFormatter.Format(_sensorReading);
            });
        }

        [Test]
        public void ThenExceptionThrownWhenNoFormatterCanHandle()
        {
            MockFormatter.CanFormat(_sensorReading).Returns(false);

            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await OutputService.WriteAsync(_sensorReading)
            );
        }
    }
}