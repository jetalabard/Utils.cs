using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.StandardSerializer;
using Utils.FileReaderWriter.Serialization.Default;

namespace UnitTest.SerializeDeserialize.Serializer
{
    [TestClass]
    public class DefaultSerializerTest
    {

        public class Test : Serializable
        {
            public string Attribute;

            public Test(Dictionary<string, object> listOfElements) : base(listOfElements)
            {
            }

            public override void Construct()
            {
                Attribute = "test";
            }
        }

        [TestMethod]
        public void ConstructeurTest()
        {
            Test t = new Test(new Dictionary<string, object>());
            Assert.AreEqual("test",t.Attribute);
        }

        [TestMethod]
        public void Deserialize()
        {
            IStandardSerializer<User> serializer = new DefaultSerializer<User>(new UserBasicSerializer());
            User user = serializer.Deserialize("Test test");
            Assert.AreEqual("Test", user.Name);
            Assert.AreEqual("test",user.Firstname);
        }

        [TestMethod]
        public void DeserializeDefault()
        {
            IStandardSerializer<User> serializer = new DefaultSerializer<User>();
            User user = serializer.Deserialize("Test test");
            Assert.AreEqual("Test", user.Name);
            Assert.AreEqual("test", user.Firstname);
        }

        [TestMethod]
        public void DeserializeListDefault()
        {
            IStandardSerializer<User> serializer = new DefaultSerializer<User>();
            UserList users = serializer.DeserializeList<UserList>("Toto Titi\r\nTata Roro\r\n");
            Assert.AreEqual("Toto", users[0].Name);
            Assert.AreEqual("Tata", users[1].Name);
            Assert.AreEqual("Titi", users[0].Firstname);
            Assert.AreEqual("Roro", users[1].Firstname);
        }

        [TestMethod]
        public void DeserializeList()
        {
            IStandardSerializer<User> serializer = new DefaultSerializer<User>(new UserBasicSerializer());
            UserList users = serializer.DeserializeList<UserList>("Toto Titi\r\nTata Roro\r\n");
            Assert.AreEqual("Toto", users[0].Name);
            Assert.AreEqual("Tata", users[1].Name);
            Assert.AreEqual("Titi", users[0].Firstname);
            Assert.AreEqual("Roro", users[1].Firstname);
        }

        [TestMethod]
        public void Serialize()
        {
            IStandardSerializer<User> serializer = new DefaultSerializer<User>(new UserBasicSerializer());
            string user = serializer.Serialize(new User("Test", "test"));
            Assert.AreEqual("Test test",user);
        }


        [TestMethod]
        public void SerializeDefault()
        {
            IStandardSerializer<User> serializer = new DefaultSerializer<User>();
            string user = serializer.Serialize(new User("Test", "test"));
            Assert.AreEqual("Test test", user);
        }

        [TestMethod]
        public void SerializeListDefault()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));
            IStandardSerializer<User> serializer = new DefaultSerializer<User>();
            string userString = serializer.SerializeList<UserList>(users);
            Assert.AreEqual("Toto Titi\r\nTata Roro\r\n", userString);
        }

        [TestMethod]
        public void SerializeList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IStandardSerializer<User> serializer = new DefaultSerializer<User>(new UserBasicSerializer());
            string userString = serializer.SerializeList<UserList>(users);
            Assert.AreEqual("Toto Titi\r\nTata Roro\r\n",userString);
        }


        


    }
}
