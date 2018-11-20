using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnitTest.SerializeDeserialize;
using Utils;
using Utils.FileManagement;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Specific;

namespace UnitTest.FileManagement
{
    [TestClass]
    public class FileManagerTest
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
        public void OpenCloseTest()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            ExcelWriter<User> writer = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            writer.Write<UserList>(users, ExcelFile);

            Process process = FileManager.OpenFile(ExcelFile);

            Assert.IsTrue(FileManager.IsFileLocked(ExcelFile));

            FileManager.CloseFile(process);

            //wait to process finished
            Thread.Sleep(500);

            Assert.IsFalse(FileManager.IsFileLocked(ExcelFile));


        }

        [TestMethod]
        public void FileIsNotOpen()
        {
           /* ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            ExcelWriter<User> writer = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            writer.Write<UserList>(users, ExcelFile);
            
            Assert.IsFalse(FileManager.IsFileLocked(ExcelFile));*/
        }

    }
}
