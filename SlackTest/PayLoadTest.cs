using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlackTools;

namespace SlackTest
{
    [TestClass]
    public class PayLoadTest
    {
        [TestMethod]
        public void ConstructPayLoad()
        {
            Payload p = new Payload("channel", "username", "text");
            Assert.AreEqual("channel", p.Channel);
            Assert.AreEqual("username", p.Username);
            Assert.AreEqual("text", p.Text);
        }
    }
}
