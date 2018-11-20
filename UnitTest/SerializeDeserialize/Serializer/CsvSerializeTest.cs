using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Utils;
using Utils.FileManagement;
using Utils.FileReaderWriter;
using Utils.FileReaderWriter.Reader;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.StandardSerializer;
using Utils.FileReaderWriter.Standard;

namespace UnitTest.SerializeDeserialize.Serializer
{
    [TestClass]
    public class CsvSerializeTest
    {
        private const string CSV = "csv";

        private static string CsvFile;


        [ClassInitialize()]
        public static void InitClass(TestContext context)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            CsvFile = Path.Combine(currentDirectory, "test." + CSV);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            FileManager.Delete(CsvFile);
        }

        [TestMethod]
        public void serializeCSV()
        {
            IStandardSerializer<User> serializer = new CsvSerializer<User>(';');
            string textSerialized = serializer.Serialize(new User("Toto", "Titi"));

            Assert.IsNotNull(textSerialized);
            Assert.AreEqual("Toto;Titi\r\n",textSerialized);
        }

        [TestMethod]
        public void serializeCSVList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IStandardSerializer<User> serializer = new CsvSerializer<User>(';', new StringList() { "Name", "Firstname" });
            string textSerialized = serializer.SerializeList<UserList>(users);
            Assert.IsNotNull(textSerialized);
            string[] lines = textSerialized.Split('\n');
            string lineHeader = lines[0];
            string line1 = lines[1];
            string line2 = lines[2];

            Assert.AreEqual("Name;Firstname\r",lineHeader);
            Assert.AreEqual("Toto;Titi\r",line1);
            Assert.AreEqual("Tata;Roro\r",line2);

        }

        [TestMethod]
        public void serializeCSVUserList()
        {
            UserList users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IStandardSerializer<User> serializer = new CsvSerializer<User>(';', new StringList() { "Name", "Firstname" });
            string textSerialized = serializer.SerializeList<UserList>(users);
            Assert.IsNotNull(textSerialized);
            string[] lines = textSerialized.Split('\n');
            string lineHeader = lines[0];
            string line1 = lines[1];
            string line2 = lines[2];

            Assert.AreEqual("Name;Firstname\r", lineHeader);
            Assert.AreEqual("Toto;Titi\r", line1);
            Assert.AreEqual("Tata;Roro\r", line2);

        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        [ExcludeFromCodeCoverage]
        public void AppendCsvFile()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write(user, CsvFile);

            User user2 = new User("tata", "tata");

            writer.Append<UserList>(user2, CsvFile);

            IReader<User> reader = new CsvReader<User>(';');
            reader.read<UserList>("invalidFile.csv");

        }


        [TestMethod]
        public void DeserializeWithHeader()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new CsvWriter<User>(';', new StringList { "Name", "Firstname" });
            writer.Write<UserList>(users, CsvFile);

            writer.Append<UserList>(new User("test", "test"), CsvFile);

            IReader<User> reader = new CsvReader<User>(';', new StringList { "Name", "Firstname" });
            Collection<User> usersList = reader.read<UserList>(CsvFile);

            Assert.AreEqual(3, usersList.Count);

        }

        [TestMethod]
        public void WriteCsv()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write(user, CsvFile);

            IReader<User> reader = new CsvReader<User>(';');
            Collection< User> usersList = reader.read<UserList>(CsvFile);

            Assert.AreEqual(user.Name, usersList[0].Name);
            Assert.AreEqual(user.Firstname, usersList[0].Firstname);

        }

        [TestMethod]
        public void AppendCsv()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write(user, CsvFile);

            User user2 = new User("tata", "tata");

            writer.Append<UserList>(user2, CsvFile);

            IReader<User> reader = new CsvReader<User>(';');
            Collection<User> usersList = reader.read<UserList>(CsvFile);

            Assert.AreEqual(2,usersList.Count);

            Assert.AreEqual(user.Name, usersList[0].Name);
            Assert.AreEqual(user.Firstname, usersList[0].Firstname);

            Assert.AreEqual(user.Name, usersList[0].Name);
            Assert.AreEqual(user.Firstname, usersList[0].Firstname);

            Assert.AreEqual(user2.Name, usersList[1].Name);
            Assert.AreEqual(user2.Firstname, usersList[1].Firstname);


        }

        [TestMethod]
        public void WriteCsvList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write<UserList>(users, CsvFile);

            IReader<User> reader = new CsvReader<User>(';');
            Collection<User> usersList = reader.read<UserList>(CsvFile);

            Assert.AreEqual(users.Count, usersList.Count);

            Assert.AreEqual(users[0].Name, usersList[0].Name);
            Assert.AreEqual(users[0].Firstname, usersList[0].Firstname);

            Assert.AreEqual(users[1].Name, usersList[1].Name);
            Assert.AreEqual(users[1].Firstname, usersList[1].Firstname);


        }


        [TestMethod]
        public void AppendCsvAndAddHeader()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';');
            //write file without header
            writer.Write(user, CsvFile);

            writer = new CsvWriter<User>(';', new StringList { "Name", "Firstname" });
            User user2 = new User("tata", "tata");
            //append user and add header 
            writer.Append<UserList>(user2, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';');
            StringList usersList = reader.readLine(CsvFile, true);

            Assert.AreEqual(3,usersList.Count());

            Assert.AreEqual("Name;Firstname",usersList.First());


        }


        [TestMethod]
        public void AppendCsvListWithHeader()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new CsvWriter<User>(';',new StringList { "Name","Firstname"});
            writer.Write<UserList>(users, CsvFile);

            ListSerializable<User> OtherUsers = new UserList();
            OtherUsers.Add(new User("lala", "lala"));
            OtherUsers.Add(new User("test", "test"));

            writer.Append<UserList>(OtherUsers, CsvFile, "users");

            IReader<User> reader = new CsvReader<User>(';',new StringList {"Name","Firstname" });
            Collection<User> usersList = reader.read<UserList>(CsvFile);

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
        public void AppendCsvListNoHeaderWriteButAddHeaderWithAppend()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write<UserList>(users, CsvFile);

            ListSerializable<User> OtherUsers = new UserList();
            OtherUsers.Add(new User("lala", "lala"));
            OtherUsers.Add(new User("test", "test"));

            writer = new CsvWriter<User>(';', new StringList { "Name", "Firstname" });
            writer.Append<UserList>(OtherUsers, CsvFile, "users");

            IReader<User> reader = new CsvReader<User>(';', new StringList { "Name", "Firstname" });
            Collection<User> usersList = reader.read<UserList>(CsvFile);

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
        public void AppendCsvList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer =new CsvWriter<User>(';');
            writer.Write<UserList>(users, CsvFile);

            ListSerializable<User> OtherUsers = new UserList();
            OtherUsers.Add(new User("lala", "lala"));
            OtherUsers.Add(new User("test", "test"));

            writer.Append<UserList>(OtherUsers, CsvFile, "users");

            IReader<User> reader = new CsvReader<User>(';');
            Collection<User> usersList = reader.read<UserList>(CsvFile);

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
        [ExpectedException(typeof(ArgumentException))]
        [ExcludeFromCodeCoverage]
        public void serializeCSVTestAccessPropertyMorePropertyThanObject()
        {
            new CsvSerializer<User>(';', new StringList() { "Name", "Firstname","Test", "Test2" }).Serialize(new User("", ""));
        }


        [TestMethod]
        [ExpectedException(typeof(FieldAccessException))]
        [ExcludeFromCodeCoverage]
        public void serializeCSVTestAccessPropertyInvalidProperty()
        {
            new CsvSerializer<User>(';', new StringList() { "Test", "Test2" }).Serialize(new User("",""));
        }





    }
}
