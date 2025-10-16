using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using IoTSensorReaderApp.Configuration;
using System.Threading.Tasks;

namespace IoTSensorReaderApp.Output
{
    public class DbOutputService : IOutputService
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public DbOutputService(IDbConfiguration dbConfig)
        {
            var client = new MongoClient(dbConfig.ConnectionString);
            var database = client.GetDatabase(dbConfig.DatabaseName);
            _collection = database.GetCollection<BsonDocument>(dbConfig.CollectionName);
        }

        public async Task WriteAsync(string message)
        {
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