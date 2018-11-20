using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using Utils;
using UnitTest.SerializeDeserialize.Serializer;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Standard;
using Utils.FileReaderWriter;
using Utils.FileReaderWriter.Reader;
using Utils.FileManagement;

namespace UnitTest.SerializeDeserialize
{
    [TestClass]
    public class SerializeTest
    {

        private const string TXT = "txt";
        private static string TxtFile;

        [ClassInitialize()]
        public static void InitClass(TestContext context)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            TxtFile = Path.Combine(currentDirectory, "test." + TXT);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            FileManager.Delete(TxtFile);
        }
        

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        [ExcludeFromCodeCoverage]
        public void AppendFileNotFile()
        {            
            new DefaultWriter<User>().Write(new User("Toto","Titi"), TxtFile);

            new DefaultWriter<User>().Append<UserList>(new User("Tata" , "Yoyo"), @"\invalidFile.txt");
        }


        [TestMethod]
        public void AppendFile()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));
            
            IWriter<User> writer = new DefaultWriter<User>(new UserBasicSerializer());//take default serializer
            writer.Write<UserList>(users, TxtFile);

            ListSerializable<User> OtherUsers = new UserList();
            OtherUsers.Add(new User("lala", "lala"));
            OtherUsers.Add(new User("test", "test"));

            writer.Append<UserList>(OtherUsers, TxtFile, "users");

            StringList lines = FileReader.ReadLines(TxtFile);
            Assert.AreEqual("Toto Titi", lines[0]);
            Assert.AreEqual("Tata Roro", lines[1]);
            Assert.AreEqual("lala lala", lines[2]);
            Assert.AreEqual("test test", lines[3]);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        [ExcludeFromCodeCoverage]
        public void AppendFileWithInvalidFile()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new DefaultWriter<User>(new UserBasicSerializer());//take default serializer
            writer.Write<UserList>(users, TxtFile);

            ListSerializable<User> OtherUsers = new UserList();
            OtherUsers.Add(new User("lala", "lala"));
            OtherUsers.Add(new User("test", "test"));

            writer.Append<UserList>(OtherUsers, "invalid.json", "users");
        }

    }
}
