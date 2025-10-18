using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Output;
using NUnit.Framework;

namespace IoTSensorReaderApp.IntegrationTests.OutputServices
{
    [TestFixture]
    public class OutputIntegrationTest : IntegrationTest
    {
        protected ConsoleOutputService ConsoleService;
        protected TemperatureFormatter TemperatureFormatter;
        protected HumidityFormatter HumidityFormatter;
        protected JsonSensorFormatter JsonFormatter;

        [SetUp]
        public void SetUp()
        {
            BaseSetUp();
            ConsoleService = new ConsoleOutputService();
            TemperatureFormatter = new TemperatureFormatter();
            HumidityFormatter = new HumidityFormatter();
            JsonFormatter = new JsonSensorFormatter();
        }
    }
}