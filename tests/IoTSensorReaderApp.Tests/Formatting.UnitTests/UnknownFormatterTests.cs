using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Models;
using NUnit.Framework;

namespace IoTSensorReaderApp.Tests.Formatting.UnitTests
{
    [TestFixture]
    public class UnknownFormatterTests : FormatterTestBase
    {
        private UnknownSensorFormatter _formatter;

        [SetUp]
        public void SetUp()
        {
            _formatter = new UnknownSensorFormatter();
        }

        [Test]
        public void CanFormatUnknownReadingReturnsTrue()
        {
            var reading = CreateUnknownReading();

            var result = _formatter.CanFormat(reading);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanFormatReturnsFalseForTemperatureReading()
        {
            var reading = CreateTemperatureReading(25.0);

            var result = _formatter.CanFormat(reading);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanFormatReturnsFalseForHumidityReading()
        {
            var reading = CreateHumidityReading(55.0);

            var result = _formatter.CanFormat(reading);

            Assert.That(result, Is.False);
        }

        [Test]
        public void FormatWithUnknownReadingReturnsCorrectFormat()
        {
            var reading = CreateUnknownReading();

            var result = _formatter.Format(reading);

            var expected = "Sensor 67890 | [Unknown Sensor Type] 17/10/2025 3:45:00\u202fPM: 65.5 (Raw: )";
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}