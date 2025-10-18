using IoTSensorReaderApp.Messaging;

namespace IoTSensorReaderApp.Tests.Messaging.UnitTests
{
    [TestFixture]
    public class JsonMessageDeserializerTest : MessagingTest
    {
        protected JsonMessageDeserializer Deserializer;

        [SetUp]
        public void SetUp()
        {
            Deserializer = new JsonMessageDeserializer();
        }
    }
}