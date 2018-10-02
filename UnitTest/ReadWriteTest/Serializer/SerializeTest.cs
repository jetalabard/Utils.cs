using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Utils.Serializer;
using Utils.ReadWrite.Serializer;

namespace UnitTest.ReadWriteTest
{
    [TestClass]
    public class SerializeTest
    {
        [TestMethod]
        public void serializeXMLList()
        {
            IList<UserSerializable> users = new List<UserSerializable>();
            users.Add(new UserSerializable("Toto", "Titi"));
            users.Add(new UserSerializable("Tata", "Roro"));

            ISerializer serializer = new XmlSerializer();
            string textSerialized = serializer.SerializeList(users);

            Assert.IsNotNull(textSerialized);
        }

        [TestMethod]
        public void serializeXML()
        {
            ISerializer serializer = new XmlSerializer();
            string textSerialized = serializer.Serialize(new UserSerializable("Toto", "Titi"));

            Assert.IsNotNull(textSerialized);
        }

        [TestMethod]
        public void serializeCSV()
        {
            ISerializer serializer = new CsvSerializer(';');
            string textSerialized = serializer.Serialize(new UserSerializable("Toto", "Titi"));

            Assert.IsNotNull(textSerialized);
            Assert.AreEqual(textSerialized, "Toto;Titi;");
        }

        [TestMethod]
        public void serializeJson()
        {
            ISerializer serializer = new JsonSerializer();
            string textSerialized = serializer.Serialize(new UserSerializable("Toto", "Titi"));

            Assert.IsNotNull(textSerialized);
        }

        [TestMethod]
        public void serializeCSVList()
        {
            IList<UserSerializable> users = new List<UserSerializable>();
            users.Add(new UserSerializable("Toto", "Titi"));
            users.Add(new UserSerializable("Tata", "Roro"));

            ISerializer serializer = new CsvSerializer(';');
            string textSerialized = serializer.SerializeList(users);

            Assert.IsNotNull(textSerialized);
        }
        
    }
}
