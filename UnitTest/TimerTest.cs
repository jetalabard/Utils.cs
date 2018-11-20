using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using System.Threading;

namespace UnitTest
{
    [TestClass]
    public class TimerTest
    {
        [TestMethod]
        public void DelayTest()
        {
            bool test = false;
            Utils.Timer.DelayAction(500, new Action(() => {
                test = true;
            }));
            Assert.IsFalse(test);

            Thread.Sleep(1000);

            Assert.IsTrue(test);
            


        }
    }
}
