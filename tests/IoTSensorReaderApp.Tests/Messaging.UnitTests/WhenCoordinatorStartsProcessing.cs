using NSubstitute;


namespace IoTSensorReaderApp.Tests.Messaging.UnitTests
{
    [TestFixture]
    public class WhenCoordinatorStartsProcessing : MessageCoordinatorTest
    {
        [Test]
        public async Task ThenCallsConsumerToReadMessages()
        {
            var cancellationToken = new CancellationToken();
            MockConsumer.ReadMessagesAsync(cancellationToken)
                       .Returns(EmptyAsyncMessages());

            await Coordinator.StartListeningAsync(cancellationToken);

            MockConsumer.Received(1).ReadMessagesAsync(cancellationToken);
        }

        [Test]
        public async Task ThenProcessesReceivedMessage()
        {
            var cancellationToken = new CancellationToken();
            var testMessage = "test message";
            var testReading = CreateExpectedSensorReading();

            MockConsumer.ReadMessagesAsync(cancellationToken)
                       .Returns(SingleAsyncMessage(testMessage));
            MockDeserializer.Deserialize(testMessage)
                           .Returns(testReading);

            await Coordinator.StartListeningAsync(cancellationToken);

            MockDeserializer.Received(1).Deserialize(testMessage);
            await MockProcessor.Received(1).ProcessMessageAsync(testReading);
        }

        private async IAsyncEnumerable<string> EmptyAsyncMessages()
        {
            await Task.Yield(); 
            yield break; 
        }

        private async IAsyncEnumerable<string> SingleAsyncMessage(string message)
        {
            await Task.Yield(); 
            yield return message; 
        }
    }
}