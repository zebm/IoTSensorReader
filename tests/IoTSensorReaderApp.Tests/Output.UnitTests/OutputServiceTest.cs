namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class OutputServiceTest
    {
        protected StringWriter CapturedOutput;
        protected TextWriter OriginalConsole;

        [SetUp]
        public void SetUp()
        {
            OriginalConsole = Console.Out;
            CapturedOutput = new StringWriter();
            Console.SetOut(CapturedOutput);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(OriginalConsole);
            CapturedOutput?.Dispose();
        }

        protected string GetCapturedOutput()
        {
            return CapturedOutput.ToString();
        }
    }
}