using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Sensors;
using IoTSensorReaderApp.Messaging;
using IoTSensorReaderApp.Processing;
using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.E2ETests.Helpers;
using IoTSensorReaderApp.E2ETests.Configuration;

namespace IoTSensorReaderApp.E2ETests
{
    [TestFixture]
    public class FullSystemE2ETest
    {
        private E2ETestConfiguration _config;
        private TestAppConfiguration _appConfig;
        private TestDbConfiguration _dbConfig;
        private DeviceSimulator _deviceSimulator;
        private MongoDbTestHelper _mongoHelper;
        private ConsoleOutputCapture _consoleCapture;
        private CancellationTokenSource? _cancellationTokenSource;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _config = new E2ETestConfiguration();
            _appConfig = new TestAppConfiguration(_config);
            _dbConfig = new TestDbConfiguration(_config);
            _deviceSimulator = new DeviceSimulator(_config.DeviceConnectionString);
            _mongoHelper = new MongoDbTestHelper(_config);
            _consoleCapture = new ConsoleOutputCapture();

            await _mongoHelper.CleanupTestDataAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _mongoHelper.CleanupTestDataAsync();
            _deviceSimulator?.Dispose();
            _mongoHelper?.Dispose();
            _consoleCapture?.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _consoleCapture.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        [Test]
        public async Task EndToEndTest_SendTemperatureAndHumidityReadings_ShouldProcessThroughEntireSystem()
        {
            var consoleOutputService = new ConsoleOutputService();
            var dbOutputService = new DbOutputService(_dbConfig);
            var compositeOutputService = new CompositeOutputService(new List<IOutputService> { consoleOutputService, dbOutputService });

            var sensorHandlers = new List<ISensorReadingHandler>
            {
                new TemperatureReadingHandler(consoleOutputService, new TemperatureFormatter()),
                new HumidityReadingHandler(consoleOutputService, new HumidityFormatter()),
                new JsonHandler(dbOutputService, new JsonSensorFormatter())
            };

            var messageProcessor = new SensorMessageProcessor(sensorHandlers, compositeOutputService);
            var eventHubConsumer = new EventHubConsumer(_appConfig);
            var deserializer = new JsonMessageDeserializer();
            var messageCoordinator = new MessageCoordinator(eventHubConsumer, messageProcessor, deserializer);

            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(_config.TestTimeoutSeconds));
            var initialCount = await _mongoHelper.GetDocumentCountAsync();

            TestContext.Out.WriteLine($"Starting E2E test - Initial MongoDB docu count: {initialCount}");
            TestContext.Out.WriteLine("Starting EventHub consumer...");

            var coordinatorTask = Task.Run(async () => 
            {
                try
                {
                    TestContext.Out.WriteLine("MessageCoordinator listening started");
                    await messageCoordinator.StartListeningAsync(_cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    TestContext.Out.WriteLine($"MessageCoordinator error: {ex.Message}");
                }
            });

            TestContext.Out.WriteLine("Waiting 15 seconds for consumer to start listening");
            await Task.Delay(15000);
            TestContext.Out.WriteLine("Consumer should be ready - start sending messages");

            var messages = new[]
            {
                new { Type = "Temperature", Value = 22.5 },
                new { Type = "Humidity", Value = 30.0 },
                new { Type = "Temperature", Value = 22.7 },
                new { Type = "Humidity", Value = 29.98 },
                new { Type = "Temperature", Value = 22.6 },
                new { Type = "Humidity", Value = 30.1 }
            };

            for (int i = 0; i < messages.Length; i++)
            {
                var msg = messages[i];
                TestContext.Out.WriteLine($"Sending {msg.Type}: {msg.Value}");
                
                if (msg.Type == "Temperature")
                {
                    await _deviceSimulator.SendTemperatureReadingAsync(msg.Value);
                }
                else
                {
                    await _deviceSimulator.SendHumidityReadingAsync(msg.Value);
                }
                TestContext.Out.WriteLine($"{msg.Type} message {i+1} sent");
            }

            TestContext.Out.WriteLine("All messages sent - start processing");
            var targetCount = (int)(initialCount + 6);
            TestContext.Out.WriteLine($"Waiting for {targetCount} total documents (current: {initialCount})");
            var documentsReceived = await _mongoHelper.WaitForDocumentsAsync(targetCount, TimeSpan.FromSeconds(45));
            
            TestContext.Out.WriteLine($"Document wait result: {documentsReceived}");
            _cancellationTokenSource.Cancel();
            TestContext.Out.WriteLine("MessageCoordinator cancelled");

            TestContext.Out.WriteLine("Checking console output...");
            var consoleOutput = _consoleCapture.GetOutput();
            TestContext.Out.WriteLine($"Console output length: {consoleOutput.Length} characters");
            
            if (!string.IsNullOrEmpty(consoleOutput))
            {
                TestContext.Out.WriteLine("Console output:");
                var lines = _consoleCapture.GetOutputLines();
                foreach (var line in lines)
                {
                    TestContext.Out.WriteLine($"  {line}");
                }
            }

            Assert.That(_consoleCapture.ContainsTemperatureReading(22.5) || 
                       _consoleCapture.ContainsTemperatureReading(22.7) || 
                       _consoleCapture.ContainsTemperatureReading(22.6), Is.True,
                "Console should contain at least one temperature reading");
            Assert.That(_consoleCapture.ContainsHumidityReading(30.0) || 
                       _consoleCapture.ContainsHumidityReading(29.98) || 
                       _consoleCapture.ContainsHumidityReading(30.1), Is.True,
                "Console should contain at least one humidity reading");
            Assert.That(_consoleCapture.ContainsDeviceId("12345"), Is.True,
                "Console should contain device ID 12345");

            Assert.That(documentsReceived, Is.True, "Should receive documents within timeout period");
            
            TestContext.Out.WriteLine("Checking MongoDB for expected documents");
            var temperatureDocuments = await _mongoHelper.FindDocumentsBySensorTypeAsync("Temperature");
            var humidityDocuments = await _mongoHelper.FindDocumentsBySensorTypeAsync("Humidity");

            TestContext.Out.WriteLine($"Found {temperatureDocuments.Count} temperature documents:");
            foreach (var doc in temperatureDocuments)
            {
                TestContext.Out.WriteLine($"  Temperature: {doc}");
            }

            TestContext.Out.WriteLine($"Found {humidityDocuments.Count} humidity documents:");
            foreach (var doc in humidityDocuments)
            {
                TestContext.Out.WriteLine($"  Humidity: {doc}");
            }

            Assert.That(temperatureDocuments.Count, Is.EqualTo(3), "Should have exactly 3 temperature readings");
            Assert.That(humidityDocuments.Count, Is.EqualTo(3), "Should have exactly 3 humidity readings");
            Assert.That(temperatureDocuments.First()["type"].AsString, Is.EqualTo("Temperature"), "Temperature type should match");
            Assert.That(humidityDocuments.First()["type"].AsString, Is.EqualTo("Humidity"), "Humidity type should match");

            TestContext.Out.WriteLine("E2E Test completed successfully");
        }
    }
}