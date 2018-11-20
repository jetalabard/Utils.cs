using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlackTools;
using SlackTools.Test;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics.CodeAnalysis;
using Utils;

namespace SlackTest
{
    [TestClass]
    public class SlackPostTest
    {
        private static string randomString;
        private static string urlWithAccessToken = "https://hooks.slack.com/services/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";
        private const string token = "xoxp-477650700320-478285918707-478289490579-564b9abc7cfac15a5b071600d597a646";


        [TestMethod]
        public void TestPostMessage()
        {
            SlackClient client = new SlackClient(urlWithAccessToken);

            Assert.IsTrue(client.PostMessage(username: "Jérémy Talabard",
                        text: randomString.Random(2),
                        channel: "#general"));       
        }


        [TestMethod]
        public void TestPostDirectMessageWithSlackApi()
        {
            try
            {
                SlackManager manager = new SlackManager(token);
                manager.sendDirectMessage(randomString.Random(5));
            }
            catch
            {
                Assert.Fail();
            }
           

        }


        [TestMethod]
        public void TestPostChannelMessageWithSlackApi()
        {
            try
            {
                SlackManager manager = new SlackManager(token);
                manager.sendChannelMessage(randomString.Random(8), "général");
            }
            catch
            {
                Assert.Fail();
            }

            
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestPostChannelMessageAync()
        {
            try
            {
                Task.WaitAll(SlackClient.IntegrateWithSlackAsync(urlWithAccessToken, randomString.Random(6)));
            }
            catch
            {
                Assert.Fail();
            }
            
        }


        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestGetMaessageChannel()
        {
            try
            {
                new SlackManager(token).Messages("général");
            }
            catch
            {
                Assert.Fail();
            }

        }



    }
}
