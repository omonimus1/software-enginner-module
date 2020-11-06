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

        [TestMethod]
        public void ExtractMessageSenderNumber()
        {
            string message = "please call me back at +447222555555";
            string expected = "+447222555555";
            int len = message.Length;
            // Act
            string result_elaboration = c.GetMobilePhoneSender(message, len);
            
            Assert.AreEqual(expected, result_elaboration);
        }

        public void ExtractSenderEmail()
        {
            string message = "Hello this an email davidepollicino2015@gmail.com E123456789";
            string email_expected = "davidepollicino2015@gmail.com";
            int i = 0;
            Assert.AreEqual(email_expected, c.GetEmailSender(message, message.Length, ref i));
        }

        [TestMethod]
        public void TestExractionMessageNature()
        {
            string message_id = "E23232323";
            char expected_id = 'E';
            string result_elaboration = c.GetMessageId(message_id);
            Assert.AreEqual(expected_id, result_elaboration[0]);
        }

        [TestMethod]
        public void TestUrlPasses()
        {
            string my_url = "http://www.github.com/omonimus1";
            Assert.IsTrue(c.IsHttpUrl(my_url));
        }

        [TestMethod]
        public void TestWrongUrlFormat()
        {
            string my_url = "htp:/ww.gibhub,com/omonimus1";
            Assert.IsFalse(c.IsHttpUrl(my_url));
        }

        [TestMethod]
        public void TestRealEmail()
        {
            string email = "davidepollicino2015@gmail.com";
            Assert.IsTrue(c.IsValidEmail(email));
        }

        [TestMethod]
        public void TestFakeEmail()
        {
            string email = "davidepollicino";
            Assert.IsFalse(c.IsValidEmail(email));
        }
    }
}
