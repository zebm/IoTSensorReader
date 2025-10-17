using IoTSensorReaderApp.Output;
using NSubstitute;


namespace IoTSensorReaderApp.Tests.Output.UnitTests
{
    [TestFixture]
    public class CompositeOutputServiceTest
    {
        protected CompositeOutputService CompositeOutputService;
        protected IOutputService MockOutputService1;
        protected IOutputService MockOutputService2;
        protected List<IOutputService> OutputServices;


        [SetUp]
        public void SetUp()
        {
            MockOutputService1 = Substitute.For<IOutputService>();
            MockOutputService2 = Substitute.For<IOutputService>();
            OutputServices = new List<IOutputService> { MockOutputService1, MockOutputService2 };
            CompositeOutputService = new CompositeOutputService(OutputServices);
        }

        
    }
}