using Bank;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace SendMessageTest
{
    [TestClass]
    public class Tests
    {
        SendMessage ciao = new SendMessage();
        [SetUp]
        public void Setup()
        {
        }

        [TestMethod]
        public void Test1()
        {
           string id = "@davide";
            string prova = "st amichia @davide";
            int len = prova.Length;
            string result = ciao.GetTwitterUserID(prova, len);
            Assert.AreEqual(id, result);
        }

        [TestMethod]
        public void Test2()
        {
            string id = "davidepollicino2015@gmail.com";
            string prova = "davidepollicino2015@gmail.com ciaone bllaaa neee";
            int len = prova.Length;
            string result = ciao.GetEmailSender(prova, len);
            Assert.AreEqual(id, result);
        }
    }
}