using System;

namespace IoTSensorReaderApp.Configuration
{
    public class DbConfiguration : IDbConfiguration
    {
        public string ConnectionString =>
            Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
            ?? "mongodb://localhost:27017"; 

        public string DatabaseName =>
            Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
            ?? "iotsensordata";

        public string CollectionName =>
            Environment.GetEnvironmentVariable("MONGODB_COLLECTION_NAME")
            ?? "sensorreadings";
    }
}