using IoTSensorReaderApp.Models;
using NUnit.Framework;
using System;

namespace IoTSensorReaderApp.Tests.Formatting.UnitTests
{
    [TestFixture]
    public class FormatterTestBase
    {
        protected SensorReading CreateTemperatureReading(double value)
        {
            return new SensorReading
            {
                SensorId = 12345,
                Type = SensorType.Temperature,
                Value = value,
                TimeStamp = DateTime.Parse("2025-10-17T10:30:00")
            };
        }

        protected SensorReading CreateHumidityReading(double value)
        {
            return new SensorReading
            {
                SensorId = 67890,
                Type = SensorType.Humidity,
                Value = value,
                TimeStamp = DateTime.Parse("2025-10-17T15:45:00")
            };
        }
    }
}