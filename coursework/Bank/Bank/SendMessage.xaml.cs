using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace Bank
{
    /// <summary>
    /// Interaction logic for SendMessage.xaml
    /// </summary>
    public partial class SendMessage : Window
    {
        private string TextMessage;

        public SendMessage()
        {
            InitializeComponent();
        }

        /*
         * txtBoxSender_TextChanger(): Will keep the form open while user is typing the sender
         */
        private void txtBoxSender_TextChanged(object sender, EventArgs e)
        {
            this.TextMessage = txtBoxSender.Text;
        }

        /*
         * TextBoxMessage_TextChange(): Will keep the form open while user is typing
         */
        private void TextBoxMessage_TextChanged(object sender, EventArgs e)
        {
            this.TextMessage = txtBoxMessage.Text;
        }
        
        /*
         * isInputEmpty() : return true if the header, Message or Both are empty, false otherwise. 
         */
        private bool isInputEmpty()
        {
            if (txtBoxMessage.Text == "" || txtBoxSender.Text == "")
                return true;
            else
                return false; 
        }

        /*
         * 
         */
        private void Button_Send_Click(object sender, RoutedEventArgs e, object JsonConvert)
        {
            if (isInputEmpty())
                MessageBox.Show("Make sure you have filled sender and message textboxes", "Validation Error");
            else
            {
                MessageBox.Show("All good", "Fine");
                // Start to validate now the messages
                // Try code to actually store the message in the json file. 
                /*var array = new[] { obj };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(array);
                string path = "../App_Data/data.json";
                //// Write that JSON to txt file,  
                //var read = System.IO.File.ReadAllText(path + "output.json");
                System.IO.File.WriteAllText(path + "output.json", json);
                return View();
                */


                //  https://stackoverflow.com/questions/16921652/how-to-write-a-json-file-in-c
                // Create an object on the go using Anonymous type, with the 3 propeties
                var message = new { header =txtBoxSender.Text, Message = txtBoxMessage.Text, category = "random_category" };
                // serialize JSON to a string and then write string to a file
                object p = File.WriteAllText(@"c:\movie.json", JsonConvert.SerializeObject(message));
                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(@"c:\movie.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, movie);
                }
            }

        }

        /*
         * 
         */
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtBoxMessage.Text = "";
            txtBoxSender.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void is_a_message()
        {
            /*
             * ⦁	SMS message bodies comprise Sender in the form of an international telephone phone number followed by the Message Text which 
             * is a maximum of 140 characters long. The Message Text message is simple text but may contain embedded “textspeak abbreviations”. 
             * Details of the textspeak abbreviations that may be embedded are supplied on Moodle in the form of a CSV file.

             */
        }

        private void is_an_email()
        {
            /*
             * ⦁	Email message bodies comprise Sender in the form of a standard email address John Smith ⦁	
             * john.smith@example.org followed by a 20 character Subject followed by the Message Text which is a
             * maximum of 1028 characters long. The Message Text message is simple text but may contain embedded 
             * hyperlinks in the form of standard URLs e.g. http:\\www.anywhere.com. Further detail of email messages 
             * is provided in 3.1.2 below.
             */


        }

        private void is_a_twittt()
        {
            /*
             * ⦁	Tweet bodies comprise Sender in the form of a Twitter ID: “@” followed by a maximum of 15 characters (e.g. @JohnSmith) and the Tweet text which is a maximum of 140 characters long. In addition to ordinary text the Tweet text may contain any of the following:
                ⦁	textspeak abbreviations (as in SMS above)
                ⦁	hashtags  - strings of characters preceded by a ‘#’ sign that are used to group posts by topic. (such as #BBCClick, #1Donice). 
                ⦁	Twitter IDs as above
             */
        }
    }
}
