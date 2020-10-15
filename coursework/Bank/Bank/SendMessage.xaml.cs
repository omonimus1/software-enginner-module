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
                var message = new { header = txtBoxSender.Text, Message = txtBoxMessage.Text, category = "random_category" };
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


        /*
         * is_a_message(): returns true if the sender id is the phone number of the
         * text message sender; 
         * Rederence international phone number format: 
         * https://www.cm.com/blog/how-to-format-international-telephone-numbers/
         */
        private bool is_a_message(string sender)
        {
            int len = sender.Length;
            // Check if phone number has more than 15 characters/digits
            if (len > 15 || sender == "")
                return false;
            // https://stackoverflow.com/questions/12884610/how-to-check-if-a-string-contains-any-letter-from-a-to-z
            bool contains_letter = sender.Any(x => !char.IsLetter(x));
            if (contains_letter == true)
                return false;
            // The phone numer has been correctly validated
            else
                return true;
        }

        /*
         * 
         */
        private bool is_an_email(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
            /*
             * ⦁	Email message bodies comprise Sender in the form of a standard email address John Smith ⦁	
             * john.smith@example.org followed by a 20 character Subject followed by the Message Text which is a
             * maximum of 1028 characters long. The Message Text message is simple text but may contain embedded 
             * hyperlinks in the form of standard URLs e.g. http:\\www.anywhere.com. Further detail of email messages 
             * is provided in 3.1.2 below.
             */


        }

        /*
         * is_a_tweet(): return true if the sender is the id of a twitter profile
         * A twitter sender starts with 'a'
         */
        private bool is_a_tweet(string sender)
        {
            if (sender[0] == '@')
                return true;
            else
                return false; 
            /*
             * ⦁	Tweet bodies comprise Sender in the form of a Twitter ID: “@” followed by a maximum of 15 characters (e.g. @JohnSmith) and the Tweet text which is a maximum of 140 characters long. In addition to ordinary text the Tweet text may contain any of the following:
                ⦁	textspeak abbreviations (as in SMS above)
                ⦁	hashtags  - strings of characters preceded by a ‘#’ sign that are used to group posts by topic. (such as #BBCClick, #1Donice). 
                ⦁	Twitter IDs as above
             */
        }
    }
}
