using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.FileManagement;
using System.IO;
using Utils;
using UnitTest.SerializeDeserialize;
using System.Diagnostics.CodeAnalysis;
using System.Collections.ObjectModel;
using Utils.FileReaderWriter.Specific;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Reader;
using System;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;

namespace UnitTest.FileManagement
{
    [TestClass]
    public class ExcelManagerTest
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

        /// <summary>
        /// Test run asynchronously but file must be close and not locked to overwrite them
        /// so this method allows to run test synchronously
        /// </summary>
        [TestMethod]
        public void ExcelTest()
        {
            AddWorkSheetsTest();
            ClearTest();

        }

        [TestMethod]
        public void ExcelInstalled()
        {
            Assert.IsTrue(ExcelManager.IsExcelInstalled());

        }



        [TestMethod]
        public void RangeToBoldTest()
        {

            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            ExcelWriter<User> writer = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            writer.Write<UserList>(users, ExcelFile);

            new ExcelManager().RangeToBold("A2:B3","Users", ExcelFile);
            
           Assert.IsTrue(new ExcelManager().CheckRangeBold("A2:B3", "Users", ExcelFile));

        }


        [TestMethod]
        public void RangeToColorTest()
        {

            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            ExcelWriter<User> writer = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            writer.Write<UserList>(users, ExcelFile);

            new ExcelManager().RangeToColor("A2:B3", System.Drawing.Color.Red,"Users", ExcelFile);

            System.Drawing.Color red = new ExcelManager().CheckRangeColor("A2:B3", "Users", ExcelFile);
            
            Assert.AreEqual(red, System.Drawing.Color.Red);
        
        }


        [TestMethod]
        public void RangeResizeTest()
        {

            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            ExcelWriter<User> writer = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            writer.Write<UserList>(users, ExcelFile);

            new ExcelManager().RangeResize("A2:B3", "Users", ExcelFile ,30.3f);

            Assert.AreEqual(30.3f, new ExcelManager().CheckRangeSize("A2:B3", "Users", ExcelFile));

        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void AddWorkSheetsTest()
        {
            ExcelManager manager = new ExcelManager();
            manager.AddWorkSheets(new StringList { "Worksheet1", "Worksheet2" }, ExcelFile);

            try
            {
                //try to go on new worksheets
                manager.ChangeCellValue(ExcelFile, "Worksheet1", "A1", "Test");
                manager.ChangeCellValue(ExcelFile, "Worksheet2", "A1", "Test");
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ClearTest()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            ExcelWriter<User> writer = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            writer.Write<UserList>(users, ExcelFile);

            new ExcelManager().Clear(ExcelFile, "Users", "A2:B3");

            IReader<User> reader = new ExcelReader<User>("Users", new StringList { "Name", "Firstname" });
            Collection<User> usersList = reader.read<UserList>(ExcelFile);

            Assert.AreEqual(0, usersList.Count);
        }


        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SaveWithError()
        {
            using (ExcelPackage excel = new ExcelPackage(new FileInfo("blabla.xlsx")))
            {
                new ExcelManager().Save(excel);
            }
            
        }



    }
}
