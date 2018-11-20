using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.SerializeDeserialize;
using Utils;
using Utils.FileManagement;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Specific;

namespace UnitTest
{
    [TestClass]
    public class ColorTest
    {
        [TestMethod]
        public void ToDrawingColorTest()
        {
            System.Drawing.Color red = System.Drawing.Color.Red;
            System.Windows.Media.Color red2 = Color.FromRgb(red.R, red.G, red.B);
            System.Drawing.Color red3 = red2.ToDrawingColor();
            Assert.AreEqual(red3.ToArgb(), red.ToArgb());
        }


        [TestMethod]
        public void ToMediaColorTest()
        {
            System.Drawing.Color red = System.Drawing.Color.Red;
            System.Windows.Media.Color red2 = Color.FromRgb(red.R, red.G, red.B);
            Assert.AreEqual(red.ToMediaColor(), red2);
            
        }


        [TestMethod]
        public void ExcelColorTest()
        {

            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            string currentDirectory = Directory.GetCurrentDirectory();
            string ExcelFile = Path.Combine(currentDirectory, "test.xlsx" );

            ExcelWriter<User> writer = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            writer.Write<UserList>(users, ExcelFile);

            ExcelColor color = null;

            using (ExcelPackage excel = new ExcelPackage(new FileInfo(ExcelFile)))
            {
                var worksheet = excel.Workbook.Worksheets["Users"];
                color = worksheet.Cells["A1:B2"].Style.Font.Color;
                color.SetColor(System.Drawing.Color.White);
            }

            
            System.Windows.Media.Color colorTransform = Color.ToMediaColor(color);
            Assert.AreEqual(colorTransform.ToDrawingColor().ToArgb(), System.Drawing.Color.White.ToArgb());

            FileManager.Delete(ExcelFile);
        }

    }
}
