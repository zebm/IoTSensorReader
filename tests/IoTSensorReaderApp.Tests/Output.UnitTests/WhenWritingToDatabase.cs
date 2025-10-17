namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class WhenWritingToDatabase : DbOutputServiceTest
    {
        [Test]
        public async Task ThenMethodCompletes()
        {
            var message = """{"type":"Temperature","value":25.5}""";

            var task = DbOutputService.WriteAsync(message);
            await task;

            Assert.That(task.IsCompletedSuccessfully, Is.True);
        }

        [Test]
        public async Task ThenCanHandleMultipleWrites()
        {
            var message1 = """{"type":"Temperature","value":25.5}""";
            var message2 = """{"type":"Humidity","value":60.0}""";

            var task1 = DbOutputService.WriteAsync(message1);
            var task2 = DbOutputService.WriteAsync(message2);
    
            await Task.WhenAll(task1, task2);

            Assert.That(task1.IsCompletedSuccessfully, Is.True);
            Assert.That(task2.IsCompletedSuccessfully, Is.True);
        }

        [Test]
        public async Task ThenInvalidJsonWritesErrorToConsole()
        {
            var invalidJson = "This is not JSON";

            await DbOutputService.WriteAsync(invalidJson);

            var consoleOutput = GetCapturedOutput();
            Assert.That(consoleOutput, Does.Contain("Failed to write to database"));
        }

        [Test]
        public async Task ThenValidJsonCompletesWithoutError()
        {
            var validJson = """{"type":"Temperature","value":25.5}""";

            await DbOutputService.WriteAsync(validJson);

            var consoleOutput = GetCapturedOutput();
            Assert.That(consoleOutput, Does.Not.Contain("Failed to write to database"));
        }
    }
}