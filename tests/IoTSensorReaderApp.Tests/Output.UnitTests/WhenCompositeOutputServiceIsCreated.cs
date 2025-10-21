using IoTSensorReaderApp.Output;
using NSubstitute;

namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class WhenCompositeOutputServiceIsCreated
    {
        [Test]
        public void ThenThrowsExceptionIfServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CompositeOutputService(null!));
        }

        [Test]
        public void ThenCanBeCreatedWithOneService()
        {
            var mockService = Substitute.For<IOutputService>();
            var services = new List<IOutputService> { mockService };

            Assert.DoesNotThrow(() => new CompositeOutputService(services));
        }

        [Test]
        public void ThenCanBeCreatedWithMultipleServices()
        {
            var mockService1 = Substitute.For<IOutputService>();
            var mockService2 = Substitute.For<IOutputService>();
            var services = new List<IOutputService> { mockService1, mockService2 };

            Assert.DoesNotThrow(() => new CompositeOutputService(services));
        }

        [Test]
        public void ThenThrowsExceptionIfServicesIsEmpty()
        {
            var emptyServices = new List<IOutputService>();

            Assert.Throws<ArgumentException>(() => new CompositeOutputService(emptyServices));
        }
    }
}