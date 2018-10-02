using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class ZipTest
    {
        [TestMethod]
        public void TestZip()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string directoryCreated = Directory.GetCurrentDirectory() + @"\data";
            Directory.CreateDirectory(directoryCreated);
            ZipUtility.ZipDirectory(directoryCreated, currentDirectory + @"\zipData.zip");

            Assert.IsTrue(File.Exists(currentDirectory + @"\zipData.zip"));
            Directory.Delete(directoryCreated);
            File.Delete(currentDirectory + @"\zipData.zip");
        }

        [TestMethod]
        public void TestUnZip()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string directoryCreated = Directory.GetCurrentDirectory() + @"\data";
            Directory.CreateDirectory(directoryCreated);
            ZipUtility.ZipDirectory(directoryCreated, currentDirectory + @"\zipData.zip");
            //remove data directory
            Directory.Delete(directoryCreated);
            //recreate data directory from zip file
            ZipUtility.UnZipDirectory(directoryCreated, currentDirectory + @"\zipData.zip");

            Assert.IsTrue(Directory.Exists(directoryCreated));
            Directory.Delete(directoryCreated);
            File.Delete(currentDirectory + @"\zipData.zip");
        }

    }
}
