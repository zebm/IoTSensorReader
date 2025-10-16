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

            await SystemUnderTest.ProcessMessageAsync(temperatureReading);

            MockTemperatureHandler.Received(1).CanHandle(temperatureReading);
            await MockTemperatureHandler.Received(1).HandleAsync(temperatureReading);
            await MockHumidityHandler.DidNotReceive().HandleAsync(Arg.Any<SensorReading>());
        }
    }
}