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
            string message = "please s1346579 call me back at +447222555555";
            string expected = "+447222555555";
            int len = message.Length;
            // Act
            string result_elaboration = c.GetMobilePhoneSender(message, len);

            Assert.AreEqual(expected, result_elaboration);
        }

        [TestMethod]
        public void TestExtendMessage()
        {
            string message = "AAP S123456789 ciaone this has a abbreviation";
            string expected = "AAP <Always a pleasure> S123456789 ciaone this has a abbreviation";
            Assert.AreEqual(expected, c.ExtendAbbreviationInsideMessage(message, message.Length)); ;
        }

        [TestMethod]
        public void ExtractEmailId()
        {
            int start_subject = 0; 
            string message = "hello the id could be E12345679, contact me at davidepollicino2015@gmail.com";
            Assert.AreEqual("davidepollicino2015@gmail.com", c.GetEmailSender(message, message.Length,ref start_subject));
        }

        [TestMethod]
        public void TestExractionMessageNature()
        {
            string message_id = "E123456789";
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
