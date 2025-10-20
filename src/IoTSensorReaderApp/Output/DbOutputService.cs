using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using IoTSensorReaderApp.Configuration;
using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Formatting;

namespace IoTSensorReaderApp.Output
{
    public class DbOutputService : IOutputService
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly JsonSensorFormatter _jsonFormatter;

        public DbOutputService(IDbConfiguration dbConfig, JsonSensorFormatter jsonFormatter)
        {
            var client = new MongoClient(dbConfig.ConnectionString);
            var database = client.GetDatabase(dbConfig.DatabaseName);
            _collection = database.GetCollection<BsonDocument>(dbConfig.CollectionName);
            _jsonFormatter = jsonFormatter;
        }

        public async Task WriteAsync(SensorReading reading)
        {
            var message = _jsonFormatter.Format(reading);
            try
            {
                var document = BsonSerializer.Deserialize<BsonDocument>(message);
                
                document["insertedAt"] = DateTime.UtcNow;
                
                await _collection.InsertOneAsync(document);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to database: {ex.Message}");
            }
        }
    }
}