using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using UnitTest.SerializeDeserialize;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Writer.Standard;
using Utils.ReadWrite.Writer;
using Utils.ReadWrite.Reader;

namespace UnitTest.Reader
{
    [TestClass]
    public class XmlReaderTest
    {

        private const string XML = "xml";
        
        private static string XmlFile;

        [ClassInitialize()]
        public static void InitClass(TestContext context)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            XmlFile = Path.Combine(currentDirectory, "test." + XML);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            File.Delete(XmlFile);
        }

        [TestMethod]
        public void TestSchemaOneUser()
        {
            string startupPath = Environment.CurrentDirectory;
            string dirData = startupPath + "\\..\\..\\SerializeDeserialize\\Deserializer\\";
            string xsdfilePath = dirData + "user.xsd";
            
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write(user, XmlFile);

            Assert.IsTrue(new XmlReader<User>("users","user","",xsdfilePath).ValidateSchema(XmlFile));
            
        }

        [TestMethod]
        public void TestSchemaListUser()
        {
            string startupPath = Environment.CurrentDirectory;
            string dirData = startupPath + "\\..\\..\\SerializeDeserialize\\Deserializer\\";
            string xsdfilePath = dirData + "users.xsd";

            ListSerializable<User> usersList = new UserList();
            usersList.Add(new User("Toto", "Titi"));
            usersList.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write<UserList>(usersList, XmlFile);

            Assert.IsTrue(new XmlReader<User>("users", "user", "", xsdfilePath).ValidateSchema(XmlFile));
            
        }


        [TestMethod]
        public void TestReadXmlList()
        {
            ListSerializable<User> usersList = new UserList();
            usersList.Add(new User("Toto", "Titi"));
            usersList.Add(new User("Tata", "Roro"));
            
            new XmlWriter<User>().Write<UserList>(usersList, XmlFile);

            IReader<User> reader = new XmlReader<User>("users", "user");
            Collection<User> users = reader.read<UserList>(XmlFile);
            Assert.AreEqual(usersList.Count(), users.Count);

            Assert.AreEqual(usersList[0].Name, users[0].Name);
            Assert.AreEqual(usersList[0].Firstname, users[0].Firstname);

            Assert.AreEqual(usersList[1].Name, users[1].Name);
            Assert.AreEqual(usersList[1].Firstname, users[1].Firstname);
            

        }

        [TestMethod]
        public void TestSchemaOneExceptionIncompatibleSchema()
        {
            string startupPath = Environment.CurrentDirectory;
            string dirData = startupPath + "\\..\\..\\SerializeDeserialize\\Deserializer\\";
            string xsdfilePath = dirData + "users.xsd";

            User user = new User("Toto", "Titi");

            IWriter<User> writer =new XmlWriter<User>();
            writer.Write(user, XmlFile);
            bool validate = new XmlReader<User>("users", "user", "", xsdfilePath).ValidateSchema(XmlFile);
            Assert.IsFalse(validate);

        }

        [TestMethod]
        public void TestLoadXmlFileFileNotFound()
        {
            XmlReader<User> reader = new XmlReader<User>("test", "test");
            Assert.IsFalse(reader.ValidateSchema("E:\\test.xml"));

        }

        [TestMethod]
        public void TestLoadXmlInvalidURI()
        {
            XmlReader<User> reader = new XmlReader<User>("test", "test");
            Assert.IsFalse(reader.ValidateSchema(""));

        }


        [TestMethod]
        public void TestReadXmlDocument()
        {
            ListSerializable<User> usersList = new UserList();
            usersList.Add(new User("Toto", "Titi"));
            usersList.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write<UserList>(usersList, XmlFile);

            XDocument doc = new XmlReader<User>("users", "user").read(XmlFile);
            Assert.IsNotNull(doc);

            Assert.AreEqual(1,doc.Descendants("users").Count());


        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestReadXmlDocumentInvalidOperationException()
        {
            ListSerializable<User> usersList = new UserList();
            usersList.Add(new User("Toto", "Titi"));
            usersList.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write<UserList>(usersList, XmlFile);

            new XmlReader<User>("users", "user").read("invalid.xml");
            
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestReadXmlErrorLoading()
        {
            ListSerializable<User> usersList = new UserList();
            usersList.Add(new User("Toto", "Titi"));
            usersList.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write<UserList>(usersList, XmlFile);

            string startupPath = Environment.CurrentDirectory;
            string dirData = startupPath + "\\..\\..\\SerializeDeserialize\\Deserializer\\";
            string xsdfilePath = dirData + "InvalidUser.xsd";

            new XmlReader<User>("users", "user","", xsdfilePath).read<UserList>(XmlFile);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [ExcludeFromCodeCoverage]
        public void TestGenericReadXmlWithInvalidFile()
        {
            ListSerializable<User> usersList = new UserList();
            usersList.Add(new User("Toto", "Titi"));
            usersList.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write<UserList>(usersList, XmlFile);

            new XmlReader<User>("tests", "test").read<UserList>(XmlFile);
           

        }



        [TestMethod]
        public void TestSchemaListExceptionIncompatibleSchema()
        {
            string startupPath = Environment.CurrentDirectory;
            string dirData = startupPath + "\\..\\..\\SerializeDeserialize\\Deserializer\\";
            string xsdfilePath = dirData + "user.xsd";

            ListSerializable<User> usersList = new UserList();
            usersList.Add(new User("Toto", "Titi"));
            usersList.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write<UserList>(usersList, XmlFile);

            bool validate =new XmlReader<User>("users", "user", "", xsdfilePath).ValidateSchema(XmlFile);
            Assert.IsFalse(validate);

        }

        [TestMethod]
        public void TestReadXml()
        {
            User user = new User("Toto", "Titi");
                        
            new XmlWriter<User>().Write(user, XmlFile);

            IReader<User> reader = new XmlReader<User>("users", "user");
            Collection<User> users = reader.read<UserList>(XmlFile);

            Assert.AreEqual(user.Name, users[0].Name);
            Assert.AreEqual(user.Firstname, users[0].Firstname);
            

        }
    }
}
