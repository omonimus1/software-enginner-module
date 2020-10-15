using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        // Path to the cvs file that contains all the possible abbreviations 
        // and their extented meaning
        private string path_abbreviation_list = "App_Data/textwords.csv";


        // Create and store all the content of the cvs file, in an unordered_map; 
        Dictionary<string, string> map = new Dictionary<string, string>(); 




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
        private bool isInputEmpty(string sender, string message)
        {
            if (sender == "" || sender == " ")
                return true;

            else if (message == "" || message == " ")
                return true; 
            else
                return false;
        }

        /*
         * get_message_nature(): return the nature of the message in base header
         * - Return  1: if the header is of a mobile phone 
         * - Return  2: if the header is an email address
         * - Return  3: if the header is from a twitter user
         * - Return 999: if the header type has not been recognised
         */
        int get_message_nature(string header)
        {
            if (is_a_message(header))
                return 1;
            else if (is_a_message(header))
                return 2;
            else if (is_a_tweet(header))
                return 3;
            else
                return 999; 
        }



        /*
         *   Button_Send_Click(): used to validate the input received
         *   and store them if necessary
         * 
         */
        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
           // Dictionary<string, string> result;
            StreamReader sr = new StreamReader(@"../../../"+path_abbreviation_list);
            string strline = "";
            string[] _values = null;
            int x = 0;
            while (!sr.EndOfStream)
            {
                x++;
                strline = sr.ReadLine();
                _values = strline.Split(',');
                if (_values.Length >= 2 && _values[0].Trim().Length > 0)
                {
                    MessageBox.Show(_values[1]);
                }
            }
            sr.Close();
            bool isExists = path_abbreviation_list.Split(',').Any(x => x == "Always a pleasure");

            if (isExists) {
                MessageBox.Show("Ottimo, c'e'");
            }
            else
                MessageBox.Show("No, non e qua'");

            string header = txtBoxSender.Text;
            string message = txtBoxMessage.Text;
            if (isInputEmpty(header, message))
            {
                MessageBox.Show("Make sure you have filled sender and message textboxes", "Validation Error");
                return;
            }
            // Start validation process
            else
            {
                // MessageBox.Show("All good", "Fine");
                int message_type = get_message_nature(header);

                // Check if message type has not been recognized. 
                if (message_type == 999)
                {
                    // Print error message, and stop the execution of the function 
                    MessageBox.Show("The nature of the message that you have innserte, has now been recognized");
                }
                int len_message = message.Length;
                if (message_type == 2 && len_message > 1028 || message_type != 2 && len_message > 140)
                {
                    MessageBox.Show("Your messsage is too long");
                    return;
                }

                // Manage twitter message (it may contains SMS abbreviation
                if (message_type == 3)
                {

                }
                // Manage text message type
                else if (message_type == 1)
                {
                    // Process storage and check if abbreviations needs to be extended
                    // Process storage and check if abbreviations needs to be extended
                    /* “Saw your message ROFL can’t wait to see you” becomes “Saw your message 
                     * ROFL<Rolls on the floor laughing> can’t wait to see you” */

                    // Expand abbreviations
                    // path_abbreviation_list

                }
                // We are managing mesage types
                else
                {
                    // Process storage and check if abbreviations needs to be extended

                    // So, iterate to all the message, and fetch all possible words. 
                    // build the possible current word while we are having UPPERCASE letter and '/' chars
                    string possible_abbreviation = "";
                    for (int i = 0; i < len_message; i++)
                    {
                        possible_abbreviation = "";
                        while (i < len_message)
                        {
                            if (message[i] >= 'A' && message[i] <= 'Z' || message[i] == '/')
                                possible_abbreviation += message[i];
                            i += 1;
                        }
                        // Now, we have the possible abbreviation; 
                        // If abbreviation is empty skype
                        if (possible_abbreviation == "" || possible_abbreviation == " ")
                            continue; 
                        // Check if abbreviation is inside the CVS file:

                    }
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
