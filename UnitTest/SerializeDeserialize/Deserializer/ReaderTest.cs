using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using Utils.ReadWrite.Reader;

namespace UnitTest.SerializeDeserialize
{
    [TestClass]
    public class ReaderTest
    {
        private const string TXT = "txt";

        private static  string txtFile;

        [ClassInitialize()]
        public static  void InitClass(TestContext context)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            txtFile = Path.Combine(currentDirectory, "test."+TXT);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            File.Delete(txtFile);
        }

      

        [TestMethod]
        public void TestReadAllLines()
        {
            var csv = new StringBuilder();
            
            var newLine = string.Format("{0},{1}", "Talabard", "Jérémy");
            csv.AppendLine(newLine);
            newLine = string.Format("{0},{1}", "Toto", "Toto");
            csv.AppendLine(newLine);
            newLine = string.Format("{0},{1}", "Titi", "Titi");
            csv.AppendLine(newLine);
            
            File.WriteAllText(txtFile, csv.ToString());

            Assert.AreEqual(3,FileReader.ReadLines(txtFile).Count);
            

        }



    }
}
