using IoTSensorReaderApp.Sensors;
using NSubstitute;

namespace IoTSensorReaderApp.Tests.Sensors.UnitTests
{
    [TestFixture]
    public class HumidityReadingHandlerTests : SensorTest
    {
        private HumidityReadingHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _handler = new HumidityReadingHandler();
        }

        [Test]
        public void CanHandleHumidityReading_ReturnsTrue()
        {
            var humidityReading = CreateHumidityReading();

            var result = _handler.CanHandle(humidityReading);

            Assert.That(result, Is.True);
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