using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryMessage;
using System.Security.Cryptography;

namespace BankTestFinal
{
    [TestClass]
    public class UnitTest1
    {
        TestClass c = new TestClass();
        [TestMethod]
        public void TestExtractionEmailSender()
        {
            string message = "Hello, i am davidepollicino2015@gmail.com and this is my email";
            string expected = "davidepollicino2015@gmail.com";
            int len = message.Length;
            // Act
            string result_elaboration = c.GetEmailSender(message, len);
            Assert.AreEqual(expected, result_elaboration);
        }

        [TestMethod]
        public void TestExtractionTwitterSender()
        {
            string message = "This is a twitter from @davidePollicino test";
            string expected = "@davidePollicino";
            int len = message.Length;
            // Act
            string result_elaboration = c.GetTwitterUserID(message, len);
            Assert.AreEqual(expected, result_elaboration);
        }

        [TestMethod]
        public void TestExtractionMessageSender()
        {
            string message = "please call me back at 7444444444";
            string expected = "7444444444";
            int len = message.Length;
            // Act
            string result_elaboration = c.GetMobilePhoneSender(message, len);
            Assert.AreEqual(expected, result_elaboration);
        }

        [TestMethod]
        public void TestExractionMessageNature()
        {
            string message_id = "E23232323";
            char expected_id = 'E';
            Assert.AreEqual(expected_id, c.get_message_nature(message_id));
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

    }
}
