using System;

namespace IoTSensorReaderApp.Models
{
    /// <summary>
    /// Represents a reading from a sensor device.
    /// </summary>
    public class SensorReading
    {
        public int SensorId { get; set; }
        
        public SensorType Type { get; set; }

        public double Value { get; set; }

        public DateTime TimeStamp { get; set; }

        public string RawMessage { get; set; }
    }
}