using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UnitTest
{
    [TestClass]
    public class StringListTest
    {
        [TestMethod]
        public void TestDefaultConstructor()
        {
            StringList list = new StringList();
            Assert.AreEqual(0, list.Count);
        }


        [TestMethod]
        public void TestConstructor()
        {
            StringList list = new StringList(new List<string>() { "test","test2" });
            Assert.AreEqual("test", list[0]);
            Assert.AreEqual("test2", list[1]);
            Assert.AreEqual(2, list.Count);
        }



        [TestMethod]
        public void TestAddRangeGenericObject()
        {
            StringList list = new StringList();
            list.AddRange(new List<GenericObject>() { new GenericObject("test"), new GenericObject("test2") });
            Assert.AreEqual("test", list[0]);
            Assert.AreEqual("test2", list[1]);
            Assert.AreEqual(2, list.Count);
        }


        [TestMethod]
        public void TestAddRangeGenericObjectWithoutToString()
        {
            StringList list = new StringList();
            list.AddRange(new List<GenericObjectWithoutToString>() { new GenericObjectWithoutToString("test"), new GenericObjectWithoutToString("test2") });
            Assert.AreEqual("UnitTest.GenericObjectWithoutToString", list[0]);
            Assert.AreEqual("UnitTest.GenericObjectWithoutToString", list[1]);
            Assert.AreEqual(2, list.Count);
        }


        [TestMethod]
        public void TestInsert()
        {
            StringList list = new StringList();
            list.Insert(0,"testInsert");
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(0, list.IndexOf("testInsert"));

            list.Insert(0, new GenericObject("testInsert2"));
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(0, list.IndexOf("testInsert2"));

            Assert.AreEqual(1, list.IndexOf("testInsert"));
            Assert.AreEqual(0, list.IndexOf("testInsert2"));
        }

        [TestMethod]
        public void TestAdd()
        {
            StringList list = new StringList();
            list.Add("testInsert");
            Assert.AreEqual(1, list.Count);
            list.Add(new GenericObject("testInsert2"));
            Assert.AreEqual(2, list.Count);
          
            Assert.AreEqual(1, list.IndexOf("testInsert2"));
        }

        [TestMethod]
        public void TestContains()
        {
            StringList list = new StringList();
            list.Add("test");
            list.Add(new GenericObject("testInsert2"));

            Assert.IsTrue(list.Contains("test"));
            Assert.IsTrue(list.Contains("testInsert2"));

            Assert.IsTrue(list.Contains(new GenericObject("testInsert2")));
        }

        [TestMethod]
        public void TestRemove()
        {
            StringList list = new StringList();
            list.Add("test");
            list.Add(new GenericObject("testInsert2"));

            Assert.IsTrue(list.Remove("test"));
            Assert.IsTrue(list.Remove(new GenericObject("testInsert2")));

            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void TestJoin()
        {
            StringList list = new StringList();
            list.Add("test");
            list.Add(new GenericObject("testInsert2"));

            string join = list.Join(";");
            Assert.AreEqual("test;testInsert2", join);

            join = list.Join(";",true);
            Assert.AreEqual("test;testInsert2;",join);
        }

        [TestMethod]
        public void TestRemoveDuplicate()
        {
            StringList list = new StringList
            {
                "test",
                "test"
            };
            StringList listWithRemoveItem = list.RemoveDuplicates();

            Assert.IsTrue(listWithRemoveItem.Count == 1);
        }


        [TestMethod]
        public void TestFilter()
        {
            StringList list = new StringList
            {
                "test",
                new GenericObject("Othertest")
            };
            StringList listFiltered = list.Filter(x => x.Equals("test"));
            Assert.IsTrue(list.Count == 2);
            Assert.IsTrue(listFiltered.Count == 1);
        }

        [TestMethod]
        public void TestSort()
        {
            StringList list = new StringList
            {
                "c",
                "a",
                "b"

            };
            list.Sort();

            Assert.AreEqual("abc",list.Join(""));
        }



        [TestMethod]
        public void TestAddStringToStartForAll()
        {
            StringList list = new StringList
            {
                "test.png",
                "test.csv",
                "test.jpg"

            };
            list.AddStringToStartForAll("C:/Users/jetalabard/");

            Assert.AreEqual("C:/Users/jetalabard/test.png", list[0]);
            Assert.AreEqual("C:/Users/jetalabard/test.csv", list[1]);
            Assert.AreEqual("C:/Users/jetalabard/test.jpg", list[2]);
        }

        [TestMethod]
        public void TestAddStringToEndForAll()
        {
            StringList list = new StringList
            {
                "test",
                "toto",
                "titi"

            };
            list.AddStringToEndForAll(".png");

            Assert.AreEqual("test.png", list[0]);
            Assert.AreEqual("toto.png", list[1]);
            Assert.AreEqual("titi.png", list[2]);
        }

    }
}
