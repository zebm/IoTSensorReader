using NSubstitute;
using IoTSensorReaderApp.Models;

namespace IoTSensorReaderApp.Tests.Processing.UnitTests
{
    [TestFixture]
    public class WhenTemperatureReadingIsProcessed : SensorMessageProcessorTest
    {
        [Test]
        public async Task ThenTemperatureHandlerIsCalled()
        {
            SetupTemperatureHandler();
            var temperatureReading = CreateTemperatureReading();

            await Processor.ProcessMessageAsync(temperatureReading);

            MockTemperatureHandler.Received(1).CanHandle(temperatureReading);
            MockTemperatureHandler.Received(1).Handle(temperatureReading);
            MockHumidityHandler.DidNotReceive().Handle(Arg.Any<SensorReading>());
        }

        [Test]
        public async Task ThenOutputServiceReceivesReading()
        {
            SetupTemperatureHandler();
            var temperatureReading = CreateTemperatureReading();

            await Processor.ProcessMessageAsync(temperatureReading);

            await MockOutputService.Received(1).WriteAsync(temperatureReading);
        }
    }
}