using IoTSensorReaderApp.Sensors;
using NSubstitute;

namespace IoTSensorReaderApp.Tests.Sensors.UnitTests
{
    [TestFixture]
    public class UnknownReadingHandlerTests : SensorTest
    {
        private UnknownSensorHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _handler = new UnknownSensorHandler();
        }

        [Test]
        public void CanHandleUnknownReadingReturnsTrue()
        {
            var unknownReading = CreateUnknownReading();

            var result = _handler.CanHandle(unknownReading);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanHandleHumidityReading_ReturnsFalse()
        {
            var humidityReading = CreateHumidityReading();

            var result = _handler.CanHandle(humidityReading);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanHandleTemperatureReading_ReturnsFalse()
        {
            var temperatureReading = CreateTemperatureReading();

            var result = _handler.CanHandle(temperatureReading);

            Assert.That(result, Is.False);
        }
    }
}