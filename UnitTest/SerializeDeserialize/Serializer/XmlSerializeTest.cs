using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Utils.FileManagement;
using Utils.FileReaderWriter;
using Utils.FileReaderWriter.Reader;
using Utils.FileReaderWriter.Reader.XML;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.StandardSerializer;
using Utils.FileReaderWriter.Standard;

namespace UnitTest.SerializeDeserialize.Serializer
{
    [TestClass]
    public class XmlSerializeTest
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
            FileManager.Delete(XmlFile);
        }

        [TestMethod]
        public void serializeXMLList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IStandardSerializer<User> serializer = new XmlSerializer<User>();
            string textSerialized = serializer.SerializeList<UserList>(users);
            string result = "<users xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><user><Firstname>Titi</Firstname><Name>Toto</Name></user><user><Firstname>Roro</Firstname><Name>Tata</Name></user></users>";
            Assert.AreEqual(textSerialized, result);
        }

        [TestMethod]
        public void serializeXML()
        {
            IStandardSerializer<User> serializer = new XmlSerializer<User>();
            string textSerialized = serializer.Serialize(new User("Toto", "Titi"));

            Assert.AreEqual("<user xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><Firstname>Titi</Firstname><Name>Toto</Name></user>", textSerialized);
        }

        [TestMethod]
        public void WriteXMLList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write<UserList>(users, XmlFile);

            IReader<User> reader = new XmlReader<User>("users", "user");
            Collection<User> usersList = reader.read<UserList>(XmlFile);

            Assert.AreEqual(users.Count, usersList.Count);

            Assert.AreEqual(users[0].Name, usersList[0].Name);
            Assert.AreEqual(users[0].Firstname, usersList[0].Firstname);

            Assert.AreEqual(users[1].Name, usersList[1].Name);
            Assert.AreEqual(users[1].Firstname, usersList[1].Firstname);



        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        [ExcludeFromCodeCoverage]
        public void AppendListXmlFile()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new XmlWriter<User>();
            writer.Append<UserList>(users, "invalidFile.xml", "users");

        }


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        [ExcludeFromCodeCoverage]
        public void AppendXmlFile()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write(user, XmlFile);

            User user2 = new User("tata", "tata");

            writer.Append<UserList>(user2, "invalidFile.xml");

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [ExcludeFromCodeCoverage]
        public void AppendXmlFileWithInvalidElementName()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write<UserList>(users, XmlFile);
            ListSerializable<User> users2 = new UserList();
            users2.Add( new User("tata", "tata"));

            writer.Append<UserList>(users2, XmlFile, "");

        }



        [TestMethod]
        public void AppendXML()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write(user, XmlFile);

            User user2 = new User("tata", "tata");

            writer.Append<UserList>(user2, XmlFile);

            IReader<User> reader = new XmlReader<User>("users", "user");
            Collection<User> usersList = reader.read<UserList>(XmlFile);

            Assert.AreEqual(2,usersList.Count);

            Assert.AreEqual(user.Name, usersList[0].Name);
            Assert.AreEqual(user.Firstname, usersList[0].Firstname);

            Assert.AreEqual(user2.Name, usersList[1].Name);
            Assert.AreEqual(user2.Firstname, usersList[1].Firstname);


        }

        [TestMethod]
        public void AppendXMLList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write<UserList>(users, XmlFile);

            ListSerializable<User> OtherUsers = new UserList();
            OtherUsers.Add(new User("lala", "lala"));
            OtherUsers.Add(new User("test", "test"));

            writer.Append<UserList>(OtherUsers, XmlFile, "users");

            IReader<User> reader = new XmlReader<User>("users", "user");
            Collection<User> usersList = reader.read<UserList>(XmlFile);

            Assert.AreEqual(4,usersList.Count);
            for (int i = 0; i < users.Count(); i++)
            {
                Assert.AreEqual(users[i].Name, usersList[i].Name);
                Assert.AreEqual(users[i].Firstname, usersList[i].Firstname);
            }

            for (int i = 0; i < OtherUsers.Count(); i++)
            {
                Assert.AreEqual(OtherUsers[i].Name, usersList[i + 2].Name);
                Assert.AreEqual(OtherUsers[i].Firstname, usersList[i + 2].Firstname);
            }


        }

        [TestMethod]
        public void WriteXML()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new XmlWriter<User>();
            writer.Write(user, XmlFile);

            IReader<User> reader = new XmlReader<User>("users", "user");
            Collection< User> usersList = reader.read<UserList>(XmlFile);


            Assert.AreEqual(user.Name, usersList[0].Name);
            Assert.AreEqual(user.Firstname, usersList[0].Firstname);


        }


    }
}
