using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;

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
        public void TestAddRangeStringlist()
        {
            StringList list = new StringList{ "test", "test2" };
            StringList list2 = new StringList { "test3" };
            list.AddRange(list2);
            Assert.AreEqual("test", list[0]);
            Assert.AreEqual("test2", list[1]);
            Assert.AreEqual("test3", list[2]);
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void TestIsNumberList()
        {
            StringList list = new StringList();
            list.AddRange(new List<GenericObject>() { new GenericObject("0"), new GenericObject("2") });
            Assert.IsTrue(list.IsListOfNumbers());

            list = new StringList();
            list.AddRange(new List<GenericObject>() { new GenericObject("0"), new GenericObject("sdvsv") });
            Assert.IsFalse(list.IsListOfNumbers());
        }

        [TestMethod]
        public void TestAddRange()
        {
            StringList list = new StringList();
            StringList list2 = new StringList();
            list.AddRange(new List<GenericObject>() { new GenericObject("test"), new GenericObject("test2") });
            list.AddRange(new List<GenericObject>() { new GenericObject("test3"), new GenericObject("test4") });
            list.AddRange(list2);

            Assert.AreEqual(4,list.Count);
            Assert.AreEqual("test", list[0]);
            Assert.AreEqual( "test2",list[1]);
            Assert.AreEqual("test3", list[2]);
            Assert.AreEqual( "test4", list[3]);

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
        [ExpectedException(typeof(ArgumentException))]
        [ExcludeFromCodeCoverage]
        public void TestAddRangeGenericObjectWithoutToString()
        {
            StringList list = new StringList();
            list.AddRange(new List<GenericObjectWithoutToString>() { new GenericObjectWithoutToString("test"), new GenericObjectWithoutToString("test2") });
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        [ExcludeFromCodeCoverage]
        public void TestEquals()
        {
            StringList list = new StringList();
            int itest = 0;
            list.Equals(itest);
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
        public void TestSum()
        {
            StringList list = new StringList
            {
                "5",
                "6"
            };
            int sum = list.Sum();
            Assert.AreEqual(11,sum);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [ExcludeFromCodeCoverage]
        public void TestSumWithNoNumbers()
        {
            StringList list = new StringList
            {
                "test",
                "test"
            };
            list.Sum();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [ExcludeFromCodeCoverage]
        public void TestSumWithNoNumbers2()
        {
            StringList list = new StringList
            {
                "test",
                "5"
            };
            list.Sum();
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


        [TestMethod]
        public void TestEncryptionSha256()
        {
            StringList list = new StringList
            {
                "test",
                "toto",
                "titi"

            };
            StringList listHashed = list.Encrypt();

            Assert.AreNotEqual("test", listHashed[0]);
            Assert.AreNotEqual("toto", listHashed[1]);
            Assert.AreNotEqual("titi", listHashed[2]);

        }

        [TestMethod]
        public void TestPatternMatchingStringSimple()
        {
            StringList list = new StringList
            {
                "test",
                "toto",
                "titi"

            };
            StringList listFiltered = list.PatternMatching("o");

            Assert.AreEqual(1,listFiltered.Count);
            Assert.AreEqual("toto", listFiltered[0]);

        }

        [TestMethod]
        public void TestPatternMatchingRegex()
        {
            StringList list = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"

            };
            StringList listFiltered = list.PatternMatching(new Regex(@"^[a-zA-Z0-9]\d{2}[a-zA-Z0-9](-\d{3}){2}[A-Za-z0-9]$"));

            Assert.AreEqual(2,listFiltered.Count);
            Assert.AreEqual("1298-673-4192", listFiltered[0]);
            Assert.AreEqual("A08Z-931-468A", listFiltered[1]);

        }

        [TestMethod]
        public void TestPatternMatchingRegexString()
        {
            StringList list = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"

            };
            StringList listFiltered = list.PatternMatchingRegexString(@"^[a-zA-Z0-9]\d{2}[a-zA-Z0-9](-\d{3}){2}[A-Za-z0-9]$");

            Assert.AreEqual(2,listFiltered.Count);
            Assert.AreEqual("1298-673-4192", listFiltered[0]);
            Assert.AreEqual("A08Z-931-468A", listFiltered[1]);

        }

        [TestMethod]
        public void TestEqualMethod()
        {
            StringList list = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"
            };

            StringList list2 = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"
            };
            
            Assert.IsTrue(list == list2);

        }

        [TestMethod]
        public void TestEqualsMethodNull()
        {
            StringList list = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"
            };

            Assert.IsFalse(list.Equals(null));
        }

        [TestMethod]
        public void TestEqualMethodNull()
        {
            StringList list = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"
            };

            Assert.IsFalse(list == null);
        }

        [TestMethod]
        public void TestNotEqualMethodNull()
        {
            StringList list = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"
            };

            Assert.IsTrue(list != null);
        }

        [TestMethod]
        public void TestEqualsMethod()
        {
            StringList list = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"
            };

            StringList list2 = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"
            };

            Assert.IsTrue(list.Equals(list2));

        }

        [TestMethod]
        public void TestNotEqualMethod()
        {
            StringList list = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A"
            };

            StringList list2 = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"
            };

            Assert.IsTrue(list != list2);

        }

        [TestMethod]
        public void TestCheckOrder()
        {
            StringList list = new StringList
            {
                 "12345-KKA-1230",
               "1298-673-4192",
                "A08Z-931-468A",
                "_A90-123-129X"

            };

            StringList list2 = new StringList
            {
                "1298-673-4192",
                "_A90-123-129X",
                "A08Z-931-468A",
                "12345-KKA-1230"
            };

            Assert.IsTrue(list != list2);

        }



    }
}
