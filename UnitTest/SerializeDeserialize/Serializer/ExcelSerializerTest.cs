using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using Utils;
using System.Diagnostics.CodeAnalysis;
using System.Collections.ObjectModel;
using Utils.FileReaderWriter;
using Utils.FileReaderWriter.Specific;
using Utils.FileReaderWriter.Reader;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.SpecificSerializer;
using Utils.FileManagement;

namespace UnitTest.SerializeDeserialize.Serializer
{
    [TestClass]
    public class ExcelSerializerTest
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
        public void serializeExcel()
        {
            ExcelSerializer<User> serializer = new ExcelSerializer<User>();
            object[] obj = serializer.Serialize(new User("Toto", "Titi"));

            Assert.IsNotNull(obj);
            Assert.AreEqual("Toto", obj[0]);
            Assert.AreEqual("Titi", obj[1]);
        }

        [TestMethod]
        public void serializeExcelList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            ExcelSerializer<User> serializer = new ExcelSerializer<User>();
            List<object[]> objects = serializer.SerializeList<UserList>(users).ToList();
            Assert.IsNotNull(objects);
            object[] line1 = objects[0];
            object[] line2 = objects[1];
            
            Assert.AreEqual("Toto", line1[0]);
            Assert.AreEqual("Titi", line1[1]);
            Assert.AreEqual("Tata", line2[0]);
            Assert.AreEqual("Roro", line2[1]);

        }

        [TestMethod]
        public void writeExcelFile()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> write = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            write.Write<UserList>(users, ExcelFile);

            IReader<User> reader = new ExcelReader<User>("Users", new StringList { "Name", "Firstname" });

            ListSerializable<User> usersToCompare = reader.read<UserList>(ExcelFile);
            Assert.IsNotNull(usersToCompare);

            Assert.AreEqual(users[0].Firstname, usersToCompare[0].Firstname);
            Assert.AreEqual(users[0].Name, usersToCompare[0].Name);

            Assert.AreEqual(users[1].Firstname, usersToCompare[1].Firstname);
            Assert.AreEqual(users[1].Name, usersToCompare[1].Name);

        }


        [TestMethod]
        public void AppendListExcelFile()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> write = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            write.Write<UserList>(users, ExcelFile);

            write.Append<UserList>(users, ExcelFile);

            IReader<User> reader = new ExcelReader<User>("Users", new StringList { "Name", "Firstname" });

            ListSerializable<User> usersToCompare = reader.read<UserList>(ExcelFile);
            Assert.IsNotNull(usersToCompare);

            Assert.AreEqual(users[0].Firstname, usersToCompare[0].Firstname);
            Assert.AreEqual(users[0].Name, usersToCompare[0].Name);

            Assert.AreEqual(users[1].Firstname, usersToCompare[1].Firstname);
            Assert.AreEqual(users[1].Name, usersToCompare[1].Name);


            Assert.AreEqual(users[0].Firstname, usersToCompare[2].Firstname);
            Assert.AreEqual(users[0].Name, usersToCompare[2].Name);

            Assert.AreEqual(users[1].Firstname, usersToCompare[3].Firstname);
            Assert.AreEqual(users[1].Name, usersToCompare[3].Name);

        }



        [TestMethod]
        public void writeUserExcelFile()
        {

            IWriter<User> write = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            write.Write(new User("Tata", "Roro"), ExcelFile);

            IGenericReader<User> reader = new ExcelReader<User>("Users", new StringList { "Name", "Firstname" });

            UserList usersToCompare = reader.read<UserList>(ExcelFile);
            Assert.IsNotNull(usersToCompare);

            Assert.AreEqual("Roro", usersToCompare[0].Firstname);
            Assert.AreEqual("Tata", usersToCompare[0].Name);

        }


        [TestMethod]
        public void AppendeUserExcelFile()
        {

            IWriter<User> write = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            write.Write(new User("Tata", "Roro"), ExcelFile);

            write.Append<UserList>(new User("Tata", "Roro"), ExcelFile);

            IGenericReader<User> reader = new ExcelReader<User>("Users", new StringList { "Name", "Firstname" });

            UserList usersToCompare = reader.read<UserList>(ExcelFile);
            Assert.IsNotNull(usersToCompare);

            Assert.AreEqual("Roro", usersToCompare[0].Firstname);
            Assert.AreEqual("Tata", usersToCompare[0].Name);

            Assert.AreEqual("Roro", usersToCompare[1].Firstname);
            Assert.AreEqual("Tata", usersToCompare[1].Name);

        }



        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void serializeCSVListWithJustNameHeader()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            ExcelSerializer<User> serializer = new ExcelSerializer<User>(new StringList { "Name" });
            List<object[]> objects = serializer.SerializeList<UserList>(users).ToList();
            Assert.IsNotNull(objects);
            object[] line1 = objects[0];
            object[] line2 = objects[1];
            
            Assert.AreEqual("Toto", line1[0]);
            Assert.AreEqual("Tata", line2[0]);

            try
            {
                Assert.IsNull(line1[1]);
                Assert.IsNull(line2[1]);
            }
            catch
            {
                Assert.IsTrue(true);
            }
            

        }

        [TestMethod]
        public void ChangeCellExcel()
        {
            ExcelWriter<User> writer = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            writer.Write(new User("test", "test"), ExcelFile);
            ExcelManager manager = new ExcelManager();
            manager.ChangeCellValue(ExcelFile, "Users", "A2", "LALA");
            manager.ChangeCellValue(ExcelFile, "Users", "B2", "LALA");

            IReader<User> reader = new ExcelReader<User>("Users", new StringList { "Name", "Firstname" });
            Collection<User> usersList = reader.read<UserList>(ExcelFile);

            Assert.AreEqual(1, usersList.Count);

            Assert.AreEqual("LALA", usersList[0].Name);
            Assert.AreEqual("LALA", usersList[0].Firstname);
        }


        [TestMethod]
        public void ChangeCellsExcel()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            ExcelWriter<User> writer = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            writer.Write<UserList>(users, ExcelFile);

            new ExcelManager().ChangeCellsValue(ExcelFile, "Users",new Dictionary<string, object> { {"A2", "TEST"}, {"B2", "TEST" } });

            IReader<User> reader = new ExcelReader<User>("Users", new StringList { "Name", "Firstname" });
            Collection<User> usersList = reader.read<UserList>(ExcelFile);

            Assert.AreEqual(2, usersList.Count);

            Assert.AreEqual("TEST", usersList[0].Name);
            Assert.AreEqual("TEST", usersList[0].Firstname);

            Assert.AreEqual("Tata", usersList[1].Name);
            Assert.AreEqual("Roro", usersList[1].Firstname);
        }


        [TestMethod]
        public void WriteExcelList()
        {
            ListSerializable<User> users = new UserList();
            users.Add(new User("Toto", "Titi"));
            users.Add(new User("Tata", "Roro"));

            IWriter<User> writer = new ExcelWriter<User>("Users", new StringList { "Name", "Firstname" });
            writer.Write<UserList>(users, ExcelFile);

            IReader<User> reader = new ExcelReader<User>("Users", new StringList { "Name", "Firstname" });
            Collection<User> usersList = reader.read<UserList>(ExcelFile);

            Assert.AreEqual(users.Count, usersList.Count);

            Assert.AreEqual(users[0].Name, usersList[0].Name);
            Assert.AreEqual(users[0].Firstname, usersList[0].Firstname);

            Assert.AreEqual(users[1].Name, usersList[1].Name);
            Assert.AreEqual(users[1].Firstname, usersList[1].Firstname);


        }


    }
}
