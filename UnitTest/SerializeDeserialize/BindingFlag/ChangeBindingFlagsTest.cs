using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using Utils.FileReaderWriter.Serialization.Default;
using Utils.FileReaderWriter.Serialization.SpecificSerializer;
using Utils.FileReaderWriter.Serialization.StandardSerializer;

namespace UnitTest.SerializeDeserialize.BindingFlag
{
    [TestClass]
    public class ChangeBindingFlagsTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [ExcludeFromCodeCoverage]
        public void ChangeToDefaultSerializer()
        {
            DefaultSerializer<User> serializer = new DefaultSerializer<User>();
            serializer.setBinding(System.Reflection.BindingFlags.NonPublic);//all fields are public so no fields return
            serializer.Serialize(new User("Toto", "Titi"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [ExcludeFromCodeCoverage]
        public void ChangeToExcelSerializer()
        {
            ExcelSerializer<User> serializer = new ExcelSerializer<User>();
            serializer.setBinding(System.Reflection.BindingFlags.NonPublic);//all fields are public so no fields return
            serializer.Serialize(new User("Toto", "Titi"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [ExcludeFromCodeCoverage]
        public void ChangeToCSvSerializer()
        {
            CsvSerializer<User> serializer = new CsvSerializer<User>(';');
            serializer.setBinding(System.Reflection.BindingFlags.NonPublic);//all fields are public so no fields return
            serializer.Serialize(new User("Toto", "Titi"));
        }
    }
}
