using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using System.Diagnostics.CodeAnalysis;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.StandardSerializer;

namespace UnitTest.SerializeDeserialize
{
    [TestClass]
    public class UserTest
    {

        internal class Test : Serializable {

            [ExcludeFromCodeCoverage]
            public Test(Dictionary<string, object> listOfElements) : base(listOfElements)
            {
            }
            [ExcludeFromCodeCoverage]
            public override void Construct()
            {
                
            }
        }

        [TestMethod]
        public void TestConstruct()
        {
            StringList listElements = new StringList() { "Name","Firstname","Age"};
            User element = (User)Activator.CreateInstance(typeof(User), new object[] { listElements });
            element.Construct();
            Assert.AreEqual("Name", element.Name);
            Assert.AreEqual("Firstname", element.Firstname);
        }


        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(MissingMethodException))]
        public void TestConstructObjectWithNoCorrectConstructor()
        {
            IStandardSerializer<Test> serializer = new CsvSerializer<Test>(';');
            serializer.Deserialize("test");

        }
    }
}
