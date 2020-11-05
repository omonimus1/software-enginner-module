using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bank
{
    [TestClass]
    public partial class UnitTest1 
    {
        [TestMethod]
        public void TestMethod1()
        {
            string message_id = "E2323232";
            string message = "ciao bewewq davidepollicino2015@gmail.com ciao bello ";
            string expected = "davidepollicino2015@gmail.com";
            int len = message.Length;
            // Act
            // string GetEmailSender(string message, int len_message)
            string result_elaboration = GetEmailSender(message, len);

            Assert.Equals(expected, result_elaboration);
        }
    }
}
