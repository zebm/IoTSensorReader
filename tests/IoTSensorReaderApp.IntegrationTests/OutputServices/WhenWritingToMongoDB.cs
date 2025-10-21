using IoTSensorReaderApp.Configuration;
using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Models;
using IoTSensorReaderApp.Output;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IoTSensorReaderApp.IntegrationTests.OutputServices
{
    [TestFixture]
    public class WhenWritingToMongoDB
    {
        private IMongoCollection<BsonDocument> _collection;
        private IMongoDatabase _database;
        private DbOutputService _dbOutputService;
        private string _testCollectionName;

        public class TestDbConfiguration : IDbConfiguration
        {
            public string ConnectionString { get; }
            public string DatabaseName { get; }
            public string CollectionName { get; }

            public TestDbConfiguration(string connectionString, string databaseName, string collectionName)
            {
                ConnectionString = connectionString;
                DatabaseName = databaseName;
                CollectionName = collectionName;
            }
        }

        [SetUp]
        public void SetUp()
        {
            var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") 
                ?? "mongodb://localhost:27017";
            
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("IoTSensorReader_IntegrationTests");
            
            _testCollectionName = $"test_readings_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
            _collection = _database.GetCollection<BsonDocument>(_testCollectionName);

            var config = new TestDbConfiguration(
                connectionString, 
                "IoTSensorReader_IntegrationTests", 
                _testCollectionName);

            var formatter = new JsonSensorFormatter();
            
            _dbOutputService = new DbOutputService(config, formatter);
        }

        [TearDown]
        public void TearDown()
        {
            _database.DropCollection(_testCollectionName);
        }

        [Test]
        public async Task ThenTemperatureReadingIsStoredInMongoDB()
        {
            var reading = new SensorReading
            {
                SensorId = 123,
                Type = SensorType.Temperature,
                Value = 22.5,
                TimeStamp = DateTime.UtcNow,
                RawMessage = "{\"test\":\"data\"}"
            };

            await _dbOutputService.WriteAsync(reading);

            var documents = await _collection.Find(new BsonDocument()).ToListAsync();
            Assert.That(documents.Count, Is.EqualTo(1));

            var doc = documents[0];
            Assert.That(doc["type"].AsString, Is.EqualTo("Temperature"));
            Assert.That(doc["value"].AsDouble, Is.EqualTo(22.5));
        }

        [Test]
        public async Task ThenHumidityReadingIsStoredInMongoDB()
        {
            var reading = new SensorReading
            {
                SensorId = 456,
                Type = SensorType.Humidity,
                Value = 65.5,
                TimeStamp = DateTime.UtcNow,
                RawMessage = "{\"test\":\"data\"}"
            };

            await _dbOutputService.WriteAsync(reading);

            var documents = await _collection.Find(new BsonDocument()).ToListAsync();
            Assert.That(documents.Count, Is.EqualTo(1));

            var doc = documents[0];
            Assert.That(doc["type"].AsString, Is.EqualTo("Humidity"));
            Assert.That(doc["value"].AsDouble, Is.EqualTo(65.5));
        }

        [Test]
        public async Task ThenUnknownReadingIsStoredInMongoDB()
        {
            var reading = new SensorReading
            {
                SensorId = 789,
                Type = SensorType.Unknown,
                Value = 1013.25,
                TimeStamp = DateTime.UtcNow,
                RawMessage = "{\"pressure\":1013.25}"
            };

            await _dbOutputService.WriteAsync(reading);

            var documents = await _collection.Find(new BsonDocument()).ToListAsync();
            Assert.That(documents.Count, Is.EqualTo(1));

            var doc = documents[0];
            Assert.That(doc["type"].AsString, Is.EqualTo("Unknown"));
            Assert.That(doc["value"].AsDouble, Is.EqualTo(1013.25));
        }

        [Test]
        public async Task ThenMultipleReadingsAreStoredSeparately()
        {
            var tempReading = new SensorReading
            {
                SensorId = 123,
                Type = SensorType.Temperature,
                Value = 22.5,
                TimeStamp = DateTime.UtcNow,
                RawMessage = "{}"
            };

            var humidityReading = new SensorReading
            {
                SensorId = 456,
                Type = SensorType.Humidity,
                Value = 65.0,
                TimeStamp = DateTime.UtcNow,
                RawMessage = "{}"
            };

            await _dbOutputService.WriteAsync(tempReading);
            await _dbOutputService.WriteAsync(humidityReading);

            var documents = await _collection.Find(new BsonDocument()).ToListAsync();
            Assert.That(documents.Count, Is.EqualTo(2));

            var tempDocs = documents.Where(d => d["type"].AsString == "Temperature").ToList();
            var humidityDocs = documents.Where(d => d["type"].AsString == "Humidity").ToList();

            Assert.That(tempDocs.Count, Is.EqualTo(1));
            Assert.That(humidityDocs.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task ThenRawMessageIsStored()
        {
            var rawMessage = "{\"sensorId\":123,\"type\":1,\"value\":22.5,\"custom\":\"data\"}";
            var reading = new SensorReading
            {
                SensorId = 123,
                Type = SensorType.Temperature,
                Value = 22.5,
                TimeStamp = DateTime.UtcNow,
                RawMessage = rawMessage
            };

            await _dbOutputService.WriteAsync(reading);

            var documents = await _collection.Find(new BsonDocument()).ToListAsync();
            var doc = documents[0];
            
            Assert.That(doc["rawMessage"].AsString, Is.EqualTo(rawMessage));
        }
    }
}