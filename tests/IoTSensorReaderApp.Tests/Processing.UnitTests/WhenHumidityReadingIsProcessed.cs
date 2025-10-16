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

            await SystemUnderTest.ProcessMessageAsync(humidityReading);

            MockHumidityHandler.Received(1).CanHandle(humidityReading);
            await MockHumidityHandler.Received(1).HandleAsync(humidityReading);
            await MockTemperatureHandler.DidNotReceive().HandleAsync(Arg.Any<SensorReading>());
        }
    }
}