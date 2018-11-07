using System;
using Utils;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void TestEncryptsha256()
        {
            string someString = "test";
            string hash = someString.Encrypt();
            
            Assert.AreNotEqual(hash, "test");
            Assert.IsTrue(hash.Length > "test".Length);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [ExcludeFromCodeCoverage]
        public void TestToNumberException()
        {
            string test = "vssdv";
            test.ToNumber();
        }

        [TestMethod]
        public void TestToNumber()
        {
            string test = "5";
            Assert.AreEqual(5,test.ToNumber());
        }

        [TestMethod]
        public void TestEncryptSha512()
        {
            string someString = "test";
            
            string hash256 = someString.Encrypt();
            string hash512 = someString.Encrypt(EncryptionMode.SHA_512);

            Assert.AreNotEqual(hash512, "test");
            Assert.IsTrue(hash512.Length > hash256.Length);
        }
    }
}
