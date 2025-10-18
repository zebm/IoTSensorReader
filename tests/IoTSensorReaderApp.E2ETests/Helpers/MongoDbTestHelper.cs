using MongoDB.Driver;
using MongoDB.Bson;
using IoTSensorReaderApp.E2ETests.Configuration;

namespace IoTSensorReaderApp.E2ETests.Helpers
{
    public class MongoDbTestHelper : IDisposable
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly E2ETestConfiguration _config;

        public MongoDbTestHelper(E2ETestConfiguration config)
        {
            _config = config;
            _mongoClient = new MongoClient(config.TestMongoDbConnectionString);
            _database = _mongoClient.GetDatabase(config.TestDatabaseName);
            _collection = _database.GetCollection<BsonDocument>(config.TestCollectionName);
        }

        public async Task CleanupTestDataAsync()
        {
            await _database.DropCollectionAsync(_config.TestCollectionName);
        }



        public async Task<List<BsonDocument>> FindDocumentsBySensorTypeAsync(string sensorType)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("type", sensorType);
            var cursor = await _collection.FindAsync(filter);
            return await cursor.ToListAsync();
        }

        public async Task<long> GetDocumentCountAsync()
        {
            return await _collection.CountDocumentsAsync(new BsonDocument());
        }

        public async Task<bool> WaitForDocumentsAsync(int expectedCount, TimeSpan timeout)
        {
            var startTime = DateTime.UtcNow;
            while (DateTime.UtcNow - startTime < timeout)
            {
                var count = await _collection.CountDocumentsAsync(new BsonDocument());
                if (count >= expectedCount) return true;
                await Task.Delay(1000);
            }
            return false;
        }

        public void Dispose() { }
    }
}