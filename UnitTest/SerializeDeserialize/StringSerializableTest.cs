using Microsoft.VisualStudio.TestTools.UnitTesting;
using Util.FileReaderWriters.Serialization;

namespace UnitTest.SerializeDeserialize
{
    [TestClass]
    public class StringSerializableTest
    {

        [TestMethod]
        public void Constructeur()
        {
            StringSerializable objectString = new StringSerializable("value");
            Assert.IsNotNull(objectString);
        }
        [TestMethod]
        public void Accessor()
        {
            StringSerializable objectString = new StringSerializable("value");
            Assert.AreEqual("value", objectString.SimpleString);
        }
    }
}
