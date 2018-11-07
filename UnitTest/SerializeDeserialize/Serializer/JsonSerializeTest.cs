using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Utils.ReadWrite.Reader;
using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Serialization.StandardSerializer;
using Utils.ReadWrite.Writer;
using Utils.ReadWrite.Writer.Standard;

namespace UnitTest.SerializeDeserialize.Serializer
{
    [TestClass]
    public class JsonSerializeTest
    {

        private const string JSON = "json";

        private static string JsonFile;

        [ClassInitialize()]
        public static void InitClass(TestContext context)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            JsonFile = Path.Combine(currentDirectory, "test." + JSON);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            File.Delete(JsonFile);
        }

        [TestMethod]
        public void serializeJson()
        {
            IStandardSerializer<User> serializer = new JsonSerializer<User>();
            string textSerialized = serializer.Serialize(new User("Toto", "Titi"));

            Assert.IsNotNull(textSerialized);
            Assert.AreEqual("{\"Firstname\":\"Titi\",\"Name\":\"Toto\"}",textSerialized);


        }

        [TestMethod]
        public void serializeJSONList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IStandardSerializer<User> serializer = new JsonSerializer<User>();
            string textSerialized = serializer.SerializeList<UserList>(users);
            string result = "[{\"Firstname\":\"Titi\",\"Name\":\"Toto\"},{\"Firstname\":\"Roro\",\"Name\":\"Tata\"}]";
            Assert.AreEqual(textSerialized, result);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        [ExcludeFromCodeCoverage]
        public void AppendJsonNotFile()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new JsonWriter<User>();
            writer.Write(user, JsonFile);

            User user2 = new User("tata", "tata");

            writer.Append<UserList>(user2, JsonFile);

            IReader<User> reader = new JsonReader<User>();
            reader.read<UserList>("invalidFile.json");

        }

        [TestMethod]
        public void WriteJson()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new JsonWriter<User>();
            writer.Write(user, JsonFile);

            IReader<User> reader = new JsonReader<User>();
            Collection<User> usersList = reader.read<UserList>(JsonFile);

            Assert.AreEqual(user.Name, usersList[0].Name);
            Assert.AreEqual(user.Firstname, usersList[0].Firstname);

        }

        [TestMethod]
        public void AppendJson()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new JsonWriter<User>();
            writer.Write(user, JsonFile);

            User user2 = new User("tata", "tata");

            writer.Append<UserList>(user2, JsonFile);

            IReader<User> reader = new JsonReader<User>();
            Collection<User> usersList = reader.read<UserList>(JsonFile);

            Assert.AreEqual(2,usersList.Count);

            Assert.AreEqual(user.Name, usersList[0].Name);
            Assert.AreEqual(user.Firstname, usersList[0].Firstname);

            Assert.AreEqual(user2.Name, usersList[1].Name);
            Assert.AreEqual(user2.Firstname, usersList[1].Firstname);

        }


        [TestMethod]
        public void WriteJsonList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new JsonWriter<User>();
            writer.Write<UserList>(users, JsonFile);

            IReader<User> reader = new JsonReader<User>();
            Collection<User> usersList = reader.read<UserList>(JsonFile);

            Assert.AreEqual(users.Count(), usersList.Count);

            Assert.AreEqual(users[0].Name, usersList[0].Name);
            Assert.AreEqual(users[0].Firstname, usersList[0].Firstname);

            Assert.AreEqual(users[1].Name, usersList[1].Name);
            Assert.AreEqual(users[1].Firstname, usersList[1].Firstname);

        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        [ExcludeFromCodeCoverage]
        public void AppendJsonListFileNotFound()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new JsonWriter<User>();
            writer.Write<UserList>(users, JsonFile);

            ListSerializable<User> OtherUsers = new UserList();
            OtherUsers.Add(new User("lala", "lala"));
            OtherUsers.Add(new User("test", "test"));

            writer.Append<UserList>(OtherUsers, "invalid.json", "users");

        }

        [TestMethod]
        public void AppendJsonList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new JsonWriter<User>();
            writer.Write<UserList>(users, JsonFile);

            ListSerializable<User> OtherUsers = new UserList();
            OtherUsers.Add(new User("lala", "lala"));
            OtherUsers.Add(new User("test", "test"));

            writer.Append<UserList>(OtherUsers, JsonFile, "users");

            IReader<User> reader = new JsonReader<User>();
            Collection<User> usersList = reader.read<UserList>(JsonFile);

            Assert.AreEqual(4, usersList.Count);
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

    }
}
