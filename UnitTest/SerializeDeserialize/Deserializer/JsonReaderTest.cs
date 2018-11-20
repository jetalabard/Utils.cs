using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Utils.FileManagement;
using Utils.FileReaderWriter.Reader;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Standard;

namespace UnitTest.SerializeDeserialize.Deserializer
{
    [TestClass]
    public class JsonReaderTest
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
            FileManager.Delete(JsonFile);
        }

        [TestMethod]
        public void TestReadJsonList()
        {
            ListSerializable<User> usersList = new UserList();
            usersList.Add(new User("Toto", "Titi"));
            usersList.Add(new User("Tata", "Roro"));

            new JsonWriter<User>().Write<UserList>(usersList, JsonFile);

            IReader<User> reader = new JsonReader<User>();
            Collection<User> users = reader.read<UserList>(JsonFile);
            Assert.AreEqual(usersList.Count(), users.Count);

            Assert.AreEqual(usersList[0].Name, users[0].Name);
            Assert.AreEqual(usersList[0].Firstname, users[0].Firstname);

            Assert.AreEqual(usersList[1].Name, users[1].Name);
            Assert.AreEqual(usersList[1].Firstname, users[1].Firstname);

        }

        [TestMethod]
        public void TestReadJson()
        {
            User user = new User("Toto", "Titi");
            
            new JsonWriter<User>().Write(user, JsonFile);

            IReader<User> reader = new JsonReader<User>();
            Collection<User> users = reader.read<UserList>(JsonFile);

            Assert.AreEqual(user.Name, users[0].Name);
            Assert.AreEqual(user.Firstname, users[0].Firstname);

        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [ExcludeFromCodeCoverage]
        public void TestReadInvalidJson()
        {

            File.WriteAllText(JsonFile, "");

            new JsonReader<User>().read<UserList>(JsonFile);

        }

    }
}
