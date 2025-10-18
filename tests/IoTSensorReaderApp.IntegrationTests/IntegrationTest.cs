namespace IoTSensorReaderApp.IntegrationTests
{
    [TestFixture]
    public class IntegrationTest
    {
        protected StringWriter CapturedConsole;
        protected TextWriter OriginalConsole;

        [SetUp]
        public void BaseSetUp()
        {
            OriginalConsole = Console.Out;
            CapturedConsole = new StringWriter();
            Console.SetOut(CapturedConsole);
        }

        [TearDown]
        public void BaseTearDown()
        {
            Console.SetOut(OriginalConsole);
            CapturedConsole?.Dispose();
        }

        protected string GetConsoleOutput()
        {
            return CapturedConsole.ToString();
        }
    }
}