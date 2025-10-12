namespace IoTSensorReaderApp.Configuration
{
    /// <summary>
    /// Define contract for accessing the app config values.
    /// </summary>
    public interface IAppConfiguration
    {
        string IoTHubConnectionString { get;  }
    }
}