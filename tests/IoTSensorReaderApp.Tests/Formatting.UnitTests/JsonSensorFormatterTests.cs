using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Models;
using NUnit.Framework;
using System.Text.Json;

namespace IoTSensorReaderApp.Tests.Formatting.UnitTests
{
    [TestFixture]
    public class JsonSensorFormatterTests : FormatterTestBase
    {
        private JsonSensorFormatter _formatter;

        [SetUp]
        public void SetUp()
        {
            _formatter = new JsonSensorFormatter();
        }

        [Test]
        public void CanFormatTemperatureReadingReturnsTrue()
        {
            var reading = CreateTemperatureReading(25.0);
            var result = _formatter.CanFormat(reading);         

            Assert.That(result, Is.True);
        }   

        [Test]
        public void CanFormatHumidityReadingReturnsTrue()
        {
            var reading = CreateHumidityReading(55.0);                          
            var result = _formatter.CanFormat(reading);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanFormatForUnknownReadingReturnsTrue()
        {
            var reading = CreateUnknownReading();
            var result = _formatter.CanFormat(reading);

            Assert.That(result, Is.True);
        }

        [Test]
        public void FormatWithTemperatureReadingReturnsValidJson()
        {
            var reading = CreateTemperatureReading(23.5);

            var result = _formatter.Format(reading);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            
            Assert.DoesNotThrow(() => JsonDocument.Parse(result));
        }

        [Test]
        public void FormatWithHumidityReadingReturnsValidJson()
        {
            var reading = CreateHumidityReading(75.0);

            var result = _formatter.Format(reading);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            
            Assert.DoesNotThrow(() => JsonDocument.Parse(result));
        }

        [Test]
        public void FormatContainsCorrectTemperatureData()
        {
            var reading = CreateTemperatureReading(20.0);

            var result = _formatter.Format(reading);

            Assert.That(result, Does.Contain("\"type\":\"Temperature\""));
            Assert.That(result, Does.Contain("\"value\":20"));
            Assert.That(result, Does.Contain("\"timestamp\":\"2025-10-17T10:30:00\""));
        }

        [Test]
        public void FormatContainsCorrectHumidityData()
        {
            var reading = CreateHumidityReading(80.5);

            var result = _formatter.Format(reading);

            Assert.That(result, Does.Contain("\"type\":\"Humidity\""));
            Assert.That(result, Does.Contain("\"value\":80.5"));
            Assert.That(result, Does.Contain("\"timestamp\":\"2025-10-17T15:45:00\""));
        }
    }
}