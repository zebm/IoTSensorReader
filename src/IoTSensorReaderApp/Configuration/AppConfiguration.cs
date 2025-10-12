using System;

namespace IoTSensorReaderApp.Configuration
{
    /// <summary>
    /// Provides app config values fron env variables - namely IOT Hub Connection String
    /// </summary>
    public class AppConfiguration : IAppConfiguration
    {
        public string IoTHubConnectionString =>
            Environment.GetEnvironmentVariable("IOT_HUB_CONNECTION_STRING")
            ?? throw new InvalidOperationException("IOT_HUB_CONNECTION_STRING not set");
    }
}