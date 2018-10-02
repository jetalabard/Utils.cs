using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;
using Utils.Reader;
using System.IO;
using System.Linq;
using System.Text;
using Utils;

namespace UnitTest
{
    [TestClass]
    public class ReaderTest
    {
        
        [TestMethod]
        public void TestReadUserCsvSemiColon()
        {

            var csv = new StringBuilder();
            
            //Suggestion made by KyleMit
            var newLine = string.Format("{0};{1}", "Talabard", "Jérémy");
            csv.AppendLine(newLine);
            newLine = string.Format("{0};{1}", "Toto", "Toto");
            csv.AppendLine(newLine);
            newLine = string.Format("{0};{1}", "Titi", "Titi");
            csv.AppendLine(newLine);

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = Path.Combine(currentDirectory, "users.csv");
            //after your loop
            File.WriteAllText(path, csv.ToString());

            List<UserSerializable> users = new CsvReader(';').read<UserSerializable>(path).Cast<UserSerializable>().ToList();

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

            //Suggestion made by KyleMit
            var newLine = string.Format("{0},{1}", "Talabard", "Jérémy");
            csv.AppendLine(newLine);
            newLine = string.Format("{0},{1}", "Toto", "Toto");
            csv.AppendLine(newLine);
            newLine = string.Format("{0},{1}", "Titi", "Titi");
            csv.AppendLine(newLine);

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = Path.Combine(currentDirectory, "users.csv");

            File.WriteAllText(path, csv.ToString());

            List<UserSerializable> users = new CsvReader(',').read<UserSerializable>(path).Cast<UserSerializable>().ToList();

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

            //Suggestion made by KyleMit

            var header = string.Format("{0},{1}", "Name", "FirstName");
            csv.AppendLine(header);
            var newLine = string.Format("{0},{1}", "Talabard", "Jérémy");
            csv.AppendLine(newLine);
            newLine = string.Format("{0},{1}", "Toto", "Toto");
            csv.AppendLine(newLine);
            newLine = string.Format("{0},{1}", "Titi", "Titi");
            csv.AppendLine(newLine);

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = Path.Combine(currentDirectory, "users.csv");

            File.WriteAllText(path, csv.ToString());

            List<UserSerializable> users = new CsvReader(',', true, new StringList { "Name", "FirstName" }).read<UserSerializable>(path).Cast<UserSerializable>().ToList();

            Assert.AreEqual("Talabard", users[0].Name);
            Assert.AreEqual("Jérémy", users[0].Firstname);
            Assert.AreEqual("Toto", users[1].Name);
            Assert.AreEqual("Toto", users[1].Firstname);
            Assert.AreEqual("Titi", users[2].Name);
            Assert.AreEqual("Titi", users[2].Firstname);
        }
    }
}
