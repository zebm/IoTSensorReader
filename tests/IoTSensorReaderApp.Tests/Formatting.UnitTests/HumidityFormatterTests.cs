using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Models;
using NUnit.Framework;

namespace IoTSensorReaderApp.Tests.Formatting.UnitTests
{
    [TestFixture]
    public class HumidityFormatterTests : FormatterTestBase
    {
        private HumidityFormatter _formatter;

        [SetUp]
        public void SetUp()
        {
            _formatter = new HumidityFormatter();
        }

        [Test]
        public void FormatWithHumidityReadingReturnsCorrectFormat()
        {
            var reading = CreateHumidityReading(65.5);

            var result = _formatter.Format(reading);

            var expected = "Sensor 67890 | [Humidity] 17/10/2025 3:45:00\u202fPM: 65.5%]";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FormatWithZeroHumidityReturnsCorrectFormat()
        {
            var reading = CreateHumidityReading(0.0);

            var result = _formatter.Format(reading);

            var expected = "Sensor 67890 | [Humidity] 17/10/2025 3:45:00\u202fPM: 0%]";
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}