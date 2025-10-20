using IoTSensorReaderApp.Sensors;
using NSubstitute;

namespace IoTSensorReaderApp.Tests.Sensors.UnitTests
{
    [TestFixture]
    public class TemperatureReadingHandlerTests : SensorTest
    {
        private TemperatureReadingHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _handler = new TemperatureReadingHandler();
        }

        [Test]
        public void CanHandleTemperatureReadingReturnsTrue()
        {
            var temperatureReading = CreateTemperatureReading();

            var result = _handler.CanHandle(temperatureReading);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanHandleHumidityReading_ReturnsFalse()
        {
            var humidityReading = CreateHumidityReading();

            var result = _handler.CanHandle(humidityReading);

            Assert.That(result, Is.False);
        }
    }
}