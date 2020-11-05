using System;
using Coursework2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankTest
{
    [TestClass]
    public class UnitTest1
    {
        SendMessage c = new SendMessage();
        
        [TestMethod]
        public void TestExtractionTwitterSender()
        {
            string message = "This is a twitter from @davidePollicino test";
            string expected_twitter_id = "@davidePollicino";
            int len = message.Length;
            // Act
            string result_elaboration = c.GetTwitterUserID(message, len);
            Assert.AreEqual(expected_twitter_id, result_elaboration);
        }
    }
}
