using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.ReadWrite.Serialization.StandardSerializer;
using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Serialization.Default;
using Utils.ReadWrite.Serialization.SpecificSerializer;

namespace UnitTest.SerializeDeserialize.BindingFlag
{
    [TestClass]
    public class ChangeBindingFlagsTest
    {
        [TestMethod]
        public void ChangeToDefaultSerializer()
        {
            DefaultSerializer<User> serializer = new DefaultSerializer<User>();
            serializer.setBinding(System.Reflection.BindingFlags.NonPublic);//all fields are public so no fields return
            string userString = serializer.Serialize(new User("Toto", "Titi"));
            Assert.AreEqual("", userString);
        }

        [TestMethod]
        public void ChangeToExcelSerializer()
        {
            ExcelSerializer<User> serializer = new ExcelSerializer<User>();
            serializer.setBinding(System.Reflection.BindingFlags.NonPublic);//all fields are public so no fields return
            object[] userObject = serializer.Serialize(new User("Toto", "Titi"));
            Assert.AreEqual(0, userObject.Length);
        }

        [TestMethod]
        public void ChangeToCSvSerializer()
        {
            CsvSerializer<User> serializer = new CsvSerializer<User>(';');
            serializer.setBinding(System.Reflection.BindingFlags.NonPublic);//all fields are public so no fields return
            string userString = serializer.Serialize(new User("Toto", "Titi"));
            Assert.AreEqual("", userString);
        }
    }
}
