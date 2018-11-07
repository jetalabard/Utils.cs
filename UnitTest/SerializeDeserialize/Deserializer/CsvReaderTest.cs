using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.SerializeDeserialize;
using System.IO;
using System.Collections.Generic;
using Utils;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Collections.ObjectModel;
using Utils.ReadWrite.Writer;
using Utils.ReadWrite.Reader;
using Utils.ReadWrite.Writer.Standard;

namespace UnitTest.Reader
{
    [TestClass]
    public class CsvReaderTest
    {
        private const string CSV = "csv";
        
        private static string CsvFile;
        

        [ClassInitialize()]
        public static void InitClass(TestContext context)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            CsvFile = Path.Combine(currentDirectory, "test." + CSV);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            File.Delete(CsvFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [ExcludeFromCodeCoverage]
        public void TestManagerHeaderReadGeneric()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write(user, CsvFile);

            IReader<User> reader = new CsvReader<User>(';', new StringList());

            reader.read<UserList>(CsvFile);
        }

        [TestMethod]
        public void TestFileHasHeader()
        {
            User user = new User("Toto", "Titi");
            StringList header = new StringList { "Name", "Firstname" };
            IWriter<User> writer = new CsvWriter<User>(';', header);
            writer.Write(user, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';', header);
            Assert.IsTrue(reader.FileHasHeader(header, CsvFile));
        }

        [TestMethod]
        public void TestFileHasNoHeader()
        {
            User user = new User("Toto", "Titi");
            StringList header = new StringList { "Name", "Firstname" };
            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write(user, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';');
            Assert.IsFalse(reader.FileHasHeader(header, CsvFile));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [ExcludeFromCodeCoverage]
        public void TestManagerHeaderRead()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write(user, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';',new StringList());
            reader.read(CsvFile);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [ExcludeFromCodeCoverage]
        public void TestManagerHeaderReadLine()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write(user, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';', new StringList());
            reader.readLine(CsvFile);
        }

        [TestMethod]
        public void TestRead()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write(user, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';');
            List<StringList> lines = reader.read(CsvFile);

            Assert.AreEqual("Toto;Titi", lines[0].Join(";"));
            
        }


        [TestMethod]
        public void TestReadWithHeader()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';',new StringList { "Name","Firstname"});
            writer.Write(user, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';', new StringList { "Name", "Firstname" });
            List<StringList> lines = reader.read(CsvFile,true);

            Assert.AreEqual("Name;Firstname", lines[0].Join(";"));
            Assert.AreEqual("Toto;Titi", lines[1].Join(";"));

        }

        [TestMethod]
        public void TestReadLine()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';');
            writer.Write(user, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';');
            StringList lines = reader.readLine(CsvFile);

            Assert.AreEqual("Toto;Titi", lines[0]);
            
        }


        [TestMethod]
        public void TestReadLineWithoutHeader()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer =new CsvWriter<User>(';', new StringList { "Name", "Firstname" });
            writer.Write(user, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';', new StringList { "Name", "Firstname" });
            StringList lines = reader.readLine(CsvFile);
            Assert.AreEqual("Toto;Titi", lines[0]);
        
        }

        [TestMethod]
        public void TestReadUserCsvSemiColon()
        {

            var csv = new StringBuilder();

            var newLine = string.Format("{0};{1}", "Talabard", "Jérémy");
            csv.AppendLine(newLine);
            newLine = string.Format("{0};{1}", "Toto", "Toto");
            csv.AppendLine(newLine);
            newLine = string.Format("{0};{1}", "Titi", "Titi");
            csv.AppendLine(newLine);

            File.WriteAllText(CsvFile, csv.ToString());

            Collection<User> users = new CsvReader<User>(';').read<UserList>(CsvFile);

            Assert.AreEqual("Talabard", users[0].Name);
            Assert.AreEqual("Jérémy", users[0].Firstname);
            Assert.AreEqual("Toto", users[1].Name);
            Assert.AreEqual("Toto", users[1].Firstname);
            Assert.AreEqual("Titi", users[2].Name);
            Assert.AreEqual("Titi", users[2].Firstname);
        }


        [TestMethod]
        public void TestReadUserCsvComma()
        {

            var csv = new StringBuilder();

            var newLine = string.Format("{0},{1}", "Talabard", "Jérémy");
            csv.AppendLine(newLine);
            newLine = string.Format("{0},{1}", "Toto", "Toto");
            csv.AppendLine(newLine);
            newLine = string.Format("{0},{1}", "Titi", "Titi");
            csv.AppendLine(newLine);


            File.WriteAllText(CsvFile, csv.ToString());

            Collection<User> users = (UserList)new CsvReader<User>(',').read<UserList>(CsvFile);

            Assert.AreEqual("Talabard", users[0].Name);
            Assert.AreEqual("Jérémy", users[0].Firstname);
            Assert.AreEqual("Toto", users[1].Name);
            Assert.AreEqual("Toto", users[1].Firstname);
            Assert.AreEqual("Titi", users[2].Name);
            Assert.AreEqual("Titi", users[2].Firstname);

        }


        [TestMethod]
        public void TestReadUserCsvWithHeader()
        {
            var csv = new StringBuilder();

            var header = string.Format("{0},{1}", "Name", "FirstName");
            csv.AppendLine(header);
            var newLine = string.Format("{0},{1}", "Talabard", "Jérémy");
            csv.AppendLine(newLine);
            newLine = string.Format("{0},{1}", "Toto", "Toto");
            csv.AppendLine(newLine);
            newLine = string.Format("{0},{1}", "Titi", "Titi");
            csv.AppendLine(newLine);

            File.WriteAllText(CsvFile, csv.ToString());

            Collection<User> users = new CsvReader<User>(',', new StringList { "Name", "FirstName" }).read<UserList>(CsvFile);

            Assert.AreEqual("Talabard", users[0].Name);
            Assert.AreEqual("Jérémy", users[0].Firstname);
            Assert.AreEqual("Toto", users[1].Name);
            Assert.AreEqual("Toto", users[1].Firstname);
            Assert.AreEqual("Titi", users[2].Name);
            Assert.AreEqual("Titi", users[2].Firstname);
        }


        [TestMethod]
        public void TestReadLineWithHeader()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';', new StringList { "Name", "Firstname" });
            writer.Write(user, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';', new StringList { "Name", "Firstname" });
            StringList lines = reader.readLine(CsvFile,true);
            Assert.AreEqual("Name;Firstname", lines[0]);
            Assert.AreEqual("Toto;Titi", lines[1]);

        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReadLineWithInvalidHeader()
        {
            User user = new User("Toto", "Titi");

            IWriter<User> writer = new CsvWriter<User>(';', new StringList { "Name", "Firstname" });
            writer.Write(user, CsvFile);

            CsvReader<User> reader = new CsvReader<User>(';', new StringList { "ERROR", "Firstname" });
            reader.readLine(CsvFile);

        }
    }
}
