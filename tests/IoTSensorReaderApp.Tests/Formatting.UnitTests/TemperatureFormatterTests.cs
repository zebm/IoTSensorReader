using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Models;
using NUnit.Framework;

namespace IoTSensorReaderApp.Tests.Formatting.UnitTests
{
    [TestFixture]
    public class TemperatureFormatterTests : FormatterTestBase
    {
        private TemperatureFormatter _formatter;

        [SetUp]
        public void SetUp()
        {
            _formatter = new TemperatureFormatter();
        }

        [Test]
        public void FormatWithTemperatureReadingReturnsCorrectFormat()
        {
            var reading = CreateTemperatureReading(22.5);

            var result = _formatter.Format(reading);

            var expected = "Sensor 12345 | [Temperature] 17/10/2025 10:30:00\u202fAM: 22.5°C]";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FormatWithZeroTemperatureReturnsCorrectFormat()
        {
            var reading = CreateTemperatureReading(0.0);

            var result = _formatter.Format(reading);

            var expected = "Sensor 12345 | [Temperature] 17/10/2025 10:30:00\u202fAM: 0°C]";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FormatWithNegativeTemperatureReturnsCorrectFormat()
        {
            var reading = CreateTemperatureReading(-10.5);

            var result = _formatter.Format(reading);

            var expected = "Sensor 12345 | [Temperature] 17/10/2025 10:30:00\u202fAM: -10.5°C]";
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}