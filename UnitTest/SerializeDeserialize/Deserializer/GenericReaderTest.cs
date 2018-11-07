using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Writer;
using Utils.ReadWrite.Writer.Standard;
using Utils.ReadWrite.Reader;
using System.IO;
using Utils;
using Utils.ReadWrite.Writer.Specific;
using UnitTest.SerializeDeserialize.Serializer;

namespace UnitTest.SerializeDeserialize.Deserializer
{
    [TestClass]
    public class GenericReaderTest
    {

        private const string CSV = "csv";
        private const string XLSX = "xlsx";
        private const string JSON = "json";
        private const string XML = "xml";
        private const string TXT = "txt";

        private static string CsvFile;
        private static string TxtFile;
        private static string XmlFile;
        private static string JsonFile;
        private static string XlsxFile;


        [ClassInitialize()]
        public static void InitClass(TestContext context)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            CsvFile = Path.Combine(currentDirectory, "test." + CSV);
            TxtFile = Path.Combine(currentDirectory, "test." + TXT);
            XmlFile = Path.Combine(currentDirectory, "test." + XML);
            JsonFile = Path.Combine(currentDirectory, "test." + JSON);
            XlsxFile = Path.Combine(currentDirectory, "test." + XLSX);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            File.Delete(CsvFile);
            File.Delete(TxtFile);
            File.Delete(XmlFile);
            File.Delete(JsonFile);
            File.Delete(XlsxFile);
        }

        [TestMethod]
        public void GenericReaderCsv()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> write = new CsvWriter<User>(';', new StringList { "Name", "Firstname" });
            write.Write<UserList>(users, CsvFile);

            IGenericReader<User> reader = new CsvReader<User>(';', new StringList { "Name", "Firstname" });

            ListSerializable<User> usersToCompare = reader.read<UserList>(CsvFile);
            Assert.IsNotNull(usersToCompare);

            Assert.AreEqual(users[0].Firstname, usersToCompare[0].Firstname);
            Assert.AreEqual(users[0].Name, usersToCompare[0].Name);

            Assert.AreEqual(users[1].Firstname, usersToCompare[1].Firstname);
            Assert.AreEqual(users[1].Name, usersToCompare[1].Name);
        }


        [TestMethod]
        public void GenericReaderDefault()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> write = new DefaultWriter<User>();
            write.Write<UserList>(users, CsvFile);

            IGenericReader<User> reader = new DefaultReader<User>();

            ListSerializable<User> usersToCompare = reader.read<UserList>(CsvFile);
            Assert.IsNotNull(usersToCompare);

            Assert.AreEqual(users[0].Firstname, usersToCompare[0].Firstname);
            Assert.AreEqual(users[0].Name, usersToCompare[0].Name);

            Assert.AreEqual(users[1].Firstname, usersToCompare[1].Firstname);
            Assert.AreEqual(users[1].Name, usersToCompare[1].Name);
        }


        [TestMethod]
        public void GenericReaderDefaultWithBasicSerializer()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> write = new DefaultWriter<User>(new UserBasicSerializer());
            write.Write<UserList>(users, CsvFile);

            IGenericReader<User> reader = new DefaultReader<User>(new UserBasicSerializer());

            ListSerializable<User> usersToCompare = reader.read<UserList>(CsvFile);
            Assert.IsNotNull(usersToCompare);

            Assert.AreEqual(users[0].Firstname, usersToCompare[0].Firstname);
            Assert.AreEqual(users[0].Name, usersToCompare[0].Name);

            Assert.AreEqual(users[1].Firstname, usersToCompare[1].Firstname);
            Assert.AreEqual(users[1].Name, usersToCompare[1].Name);
        }

        [TestMethod]
        public void GenericReaderXml()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> write = new XmlWriter<User>();
            write.Write<UserList>(users, XmlFile);

            IGenericReader<User> reader = new XmlReader<User>("users", "user");

            ListSerializable<User> usersToCompare = reader.read<UserList>(XmlFile);
            Assert.IsNotNull(usersToCompare);

            Assert.AreEqual(users[0].Firstname, usersToCompare[0].Firstname);
            Assert.AreEqual(users[0].Name, usersToCompare[0].Name);

            Assert.AreEqual(users[1].Firstname, usersToCompare[1].Firstname);
            Assert.AreEqual(users[1].Name, usersToCompare[1].Name);
        }

        [TestMethod]
        public void GenericReaderJson()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> write = new JsonWriter<User>();
            write.Write<UserList>(users, JsonFile);

            IGenericReader<User> reader = new JsonReader<User>();

            ListSerializable<User> usersToCompare = reader.read<UserList>(JsonFile);
            Assert.IsNotNull(usersToCompare);

            Assert.AreEqual(users[0].Firstname, usersToCompare[0].Firstname);
            Assert.AreEqual(users[0].Name, usersToCompare[0].Name);

            Assert.AreEqual(users[1].Firstname, usersToCompare[1].Firstname);
            Assert.AreEqual(users[1].Name, usersToCompare[1].Name);
        }


        [TestMethod]
        public void GenericReaderExcel()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> write = new ExcelWriter<User>("users",new StringList {"Name","Firstname" });
            write.Write<UserList>(users, XlsxFile);

            IGenericReader<User> reader = new ExcelReader<User>("users", new StringList { "Name", "Firstname" });

            ListSerializable<User> usersToCompare = reader.read<UserList>(XlsxFile);
            Assert.IsNotNull(usersToCompare);

            Assert.AreEqual(users[0].Firstname, usersToCompare[0].Firstname);
            Assert.AreEqual(users[0].Name, usersToCompare[0].Name);

            Assert.AreEqual(users[1].Firstname, usersToCompare[1].Firstname);
            Assert.AreEqual(users[1].Name, usersToCompare[1].Name);
        }
    }
}
