using NSubstitute;
using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Tests.Processing.UnitTests
{
    [TestFixture]
    public class WhenUnknownReadingIsProcessed : SensorMessageProcessorTest
    {
        [Test]
        public async Task ThenUnknownHandlerIsCalled()
        {
            SetupUnknownHandler();
            var unknownReading = CreateUnknownReading();

            await Processor.ProcessMessageAsync(unknownReading);

            MockUnknownHandler.Received(1).CanHandle(unknownReading);
            MockUnknownHandler.Received(1).Handle(unknownReading);
            MockTemperatureHandler.DidNotReceive().Handle(Arg.Any<SensorReading>());
            MockHumidityHandler.DidNotReceive().Handle(Arg.Any<SensorReading>());
        }

        [Test]
        public async Task ThenOutputServiceReceivesReading()
        {
            SetupUnknownHandler();
            var unknownReading = CreateUnknownReading();

            await Processor.ProcessMessageAsync(unknownReading);

            await MockOutputService.Received(1).WriteAsync(unknownReading);
        }
    }
}