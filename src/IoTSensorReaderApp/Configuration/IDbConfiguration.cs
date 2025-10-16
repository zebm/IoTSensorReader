namespace IoTSensorReaderApp.Configuration
{
    public interface IDbConfiguration
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
        string CollectionName { get; }
    }
}