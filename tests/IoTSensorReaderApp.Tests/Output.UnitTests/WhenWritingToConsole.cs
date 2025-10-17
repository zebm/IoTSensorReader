namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class WhenWritingToConsole : ConsoleOutputServiceTest
    {
        [Test]
        public async Task ThenMessageIsWrittenToConsole()
        {
            var message = "Test message";

            await OutputService.WriteAsync(message);

            var output = GetCapturedOutput();
            Assert.That(output, Does.Contain(message));
        }

        [Test]
        public async Task ThenMultipleMessagesAreWrittenInOrder()
        {
            var firstMessage = "First message";
            var secondMessage = "Second message";

            await OutputService.WriteAsync(firstMessage);
            await OutputService.WriteAsync(secondMessage);

            var output = GetCapturedOutput();
            Assert.That(output, Does.Contain(firstMessage));
            Assert.That(output, Does.Contain(secondMessage));
            Assert.That(output.IndexOf(firstMessage), Is.LessThan(output.IndexOf(secondMessage)));
        }
    }
}