using IoTSensorReaderApp.Output;

namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class WhenCompositeOutputServiceIsCreated : CompositeOutputServiceTest
    {
        [Test]
        public async Task ThenThrowsExceptionForNullServices()
        {
            Assert.Throws<System.ArgumentNullException>(() => new CompositeOutputService(null));
        }

        [Test]
        public void ThenAcceptsSingleService()
        {
            var mockService = NSubstitute.Substitute.For<IOutputService>();
            var services = new List<IOutputService> { mockService };

            Assert.DoesNotThrow(() => new CompositeOutputService(services));
        }
    }
}