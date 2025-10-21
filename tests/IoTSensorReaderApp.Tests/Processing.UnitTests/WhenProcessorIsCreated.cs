using IoTSensorReaderApp.Output;
using IoTSensorReaderApp.Processing;
using IoTSensorReaderApp.Sensors;
using NSubstitute;
using NUnit.Framework;

namespace IoTSensorReaderApp.Tests.Processing.UnitTests
{
    [TestFixture]
    public class WhenProcessorIsCreated
    {
        [Test]
        public void WithValidArgumentsThenProcessorIsCreated()
        {
            var mockOutput = Substitute.For<IOutputService>();
            var mockHandler = Substitute.For<ISensorReadingHandler>();
            var handlers = new List<ISensorReadingHandler> { mockHandler };

            Assert.DoesNotThrow(() => 
                new SensorMessageProcessor(handlers, mockOutput));
        }

        [Test]
        public void WithNullHandlersThenThrowsArgumentNullException()
        {
            var mockOutput = Substitute.For<IOutputService>();

            var ex = Assert.Throws<ArgumentNullException>(() =>
                new SensorMessageProcessor(null!, mockOutput));

            Assert.That(ex.ParamName, Is.EqualTo("handlers"));
        }

        [Test]
        public void WithNullOutputServiceThenThrowsArgumentNullException()
        {
            var mockHandler = Substitute.For<ISensorReadingHandler>();
            var handlers = new List<ISensorReadingHandler> { mockHandler };

            var ex = Assert.Throws<ArgumentNullException>(() =>
                new SensorMessageProcessor(handlers, null!));

            Assert.That(ex.ParamName, Is.EqualTo("output"));
        }

        [Test]
        public void WithEmptyHandlersListThenThrowsArgumentException()
        {
            var mockOutput = Substitute.For<IOutputService>();
            var emptyHandlers = new List<ISensorReadingHandler>();

            var ex = Assert.Throws<ArgumentException>(() =>
                new SensorMessageProcessor(emptyHandlers, mockOutput));

            Assert.That(ex.Message, Does.Contain("At least one handler must be provided. (Parameter 'handlers')"));
        }
    }
}