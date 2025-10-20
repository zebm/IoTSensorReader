using IoTSensorReaderApp.Formatting;
using IoTSensorReaderApp.Output;
using NSubstitute;

namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class ConsoleOutputServiceTest : OutputServiceTest
    {
        protected ConsoleOutputService OutputService;
        protected ISensorFormatter MockFormatter;

        [SetUp]
        public void TestSetUp()
        {
            base.SetUp();
            MockFormatter = Substitute.For<ISensorFormatter>();
            var formatters = new List<ISensorFormatter> { MockFormatter };
            OutputService = new ConsoleOutputService(formatters);
        }
    }
}