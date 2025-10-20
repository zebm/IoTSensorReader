namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class OutputServiceTest
    {
        protected StringWriter ConsoleOutput;
        protected TextWriter OriginalConsole;

        [SetUp]
        public void SetUp()
        {
            OriginalConsole = Console.Out;
            ConsoleOutput = new StringWriter();
            Console.SetOut(ConsoleOutput);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(OriginalConsole);
            ConsoleOutput?.Dispose();
        }

        protected string GetCapturedOutput()
        {
            return ConsoleOutput.ToString();
        }
    }
}