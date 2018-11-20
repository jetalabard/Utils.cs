using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using Utils.FileReaderWriter;
using Utils.FileReaderWriter.Reader;
using Utils.FileReaderWriter.Specific;
using Utils.FileReaderWriter.Serialization.SpecificSerializer;
using Utils.FileManagement;

namespace UnitTest.SerializeDeserialize.Deserializer
{
    [TestClass]
    public class ExcelReaderTest
    {

        private const string XLSX = "xlsx";

        private static string ExcelFile;


        [ClassInitialize()]
        public static void InitClass(TestContext context)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            ExcelFile = Path.Combine(currentDirectory, "test." + XLSX);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            FileManager.Delete(ExcelFile);
        }


        [TestMethod]
        public void Deserialize()
        {
            IWriter<User> write = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            write.Write(new User("Tata", "Roro"), ExcelFile);

            IGenericReader<User> reader = new ExcelReader<User>("Users", new StringList { "Name", "Firstname" });

            UserList usersToCompare = reader.read<UserList>(ExcelFile);
            Assert.IsNotNull(usersToCompare);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentException))]
        public void DeserializeWithOtherThatRow()
        {
            new ExcelSerializer<User>().Deserialize(new User("",""));
        }
    }
}
