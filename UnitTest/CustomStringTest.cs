using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UnitTest
{
    [TestClass]
    public class CustomStringTest
    {
        [TestMethod]
        public void TestIsNumber()
        {
            string someString = "5";
            bool IsNumber = someString.IsNumber();
           
            Assert.IsTrue(IsNumber);

            someString = "sfvs";
            IsNumber = someString.IsNumber();

            Assert.IsFalse(IsNumber);
        }

        [TestMethod]
        public void TestBadEmail()
        {
            string someString = "test";
            
            Assert.IsFalse(someString.IsEmail());
        }

        [TestMethod]
        public void TestValidEmail()
        {
            string someString = "test.test@gmail.com";
            Assert.IsTrue(someString.IsEmail());
        }

        [TestMethod]
        public void TestEncrypt()
        {
            string someString = "test";
            string hash = someString.Encrypt();
            
            Assert.AreNotEqual(hash, "test");
            Assert.IsTrue(hash.Length > "test".Length);
        }
    }
}
