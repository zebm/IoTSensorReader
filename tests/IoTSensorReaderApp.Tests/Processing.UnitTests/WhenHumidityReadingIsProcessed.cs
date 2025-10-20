using NSubstitute;
using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Tests.Processing.UnitTests
{
    [TestFixture]
    public class WhenHumidityReadingIsProcessed : SensorMessageProcessorTest
    {
        [Test]
        public async Task ThenHumidityHandlerIsCalled()
        {
            SetupHumidityHandler();
            var humidityReading = CreateHumidityReading();

            await Processor.ProcessMessageAsync(humidityReading);

            MockHumidityHandler.Received(1).CanHandle(humidityReading);
            MockHumidityHandler.Received(1).Handle(humidityReading);
            MockTemperatureHandler.DidNotReceive().Handle(Arg.Any<SensorReading>());
        }

        [Test]
        public async Task ThenOutputServiceReceivesReading()
        {
            SetupHumidityHandler();
            var humidityReading = CreateHumidityReading();

            await Processor.ProcessMessageAsync(humidityReading);

            await MockOutputService.Received(1).WriteAsync(humidityReading);
        }
    }
}