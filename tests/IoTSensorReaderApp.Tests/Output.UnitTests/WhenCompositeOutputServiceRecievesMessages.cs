using NSubstitute;

namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class WhenCompositeOutputServiceRecievesMessages : CompositeOutputServiceTest
    {
        [Test]
        public async Task ThenAllOutputServicesAreCalled()
        {
            var message = "Test message";

            await CompositeOutputService.WriteAsync(message);

            await MockOutputService1.Received(1).WriteAsync(message);
            await MockOutputService2.Received(1).WriteAsync(message);
        }

        [Test]
        public async Task ThenServicesCalledInOrder()
        {
            var message = "Test message";

            await CompositeOutputService.WriteAsync(message);

            Received.InOrder(async () =>
            {
                await MockOutputService1.WriteAsync(message);
                await MockOutputService2.WriteAsync(message);
            });
        }
    }
}