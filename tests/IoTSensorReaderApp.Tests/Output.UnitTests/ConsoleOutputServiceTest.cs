using IoTSensorReaderApp.Output;

namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class ConsoleOutputServiceTest : OutputServiceTest
    {
        protected ConsoleOutputService OutputService;

        [SetUp]
        public void TestSetUp()
        {
            base.SetUp();
            OutputService = new ConsoleOutputService();
        }
    }
}