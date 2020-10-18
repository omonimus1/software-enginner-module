using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
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
        private string path_storage_messages = "App_Data/data.json";

        // Existing Category of important email 
        string[] urgent_email_categories = { "theft", "staff attack", "ATM theft", "raid", "customer attack", "staff abuse", "bom threat", "terrorism",
            "suspicious incident", "intelligence", "cash loss"};

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
            if (string.IsNullOrWhiteSpace(sender) || string.IsNullOrWhiteSpace(message) )
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
        int  get_message_nature(string header)
        {
            if (is_a_message(header))
                return 1;
            else if (is_an_email(header))
            {
                return 2;
            }
            else if (is_a_tweet(header))
                return 3;
            else
                return 999; 
        }

        /*
        string get_email_type(char header)
        {
            // Check for Significant Incident Reports
            // Significant Incident Reports will have the subject iont he form: "SIR dd/mm/yy"

            // create and return a pair<string, string>: <email_type-category>

            // This zero, will be the index of the array of impiortant email categories; 
            //Tuple<string, int> t = new Tuple<string, int>("Hello", 0);
            
        }*/

        string extend_any_abbreviation(string message, int len_message)
        {
            // Check if abbreviation is inside the CVS file:
            StreamReader sr = new StreamReader(@"../../../" + path_abbreviation_list);
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
                if (string.IsNullOrWhiteSpace(possible_abbreviation) || possible_abbreviation.Length == 1) 
                    continue;
                string strline = "";
                string[] _values = null;
                int x = 0;
                while (!sr.EndOfStream)
                {
                    x++;
                    strline = sr.ReadLine();
                    _values = strline.Split(',');
                    // print Any day Now, the extension of ADN
                    // thjebug weith "ADN" instead possible_abbreviation eventually
                    if (_values[0] == possible_abbreviation)
                    {
                        MessageBox.Show(_values[1]);
                        // Here add the enxtation with this structure: <FullExtansion>
                        string extended_abbreviation = " <" + _values[1] + "> ";
                        // Add the entended appreviation at the position of where we found them 
                        message.Insert(i, extended_abbreviation);
                    }
                }
                
            }
            // Close file stream after have checked all possible abbreviations
            sr.Close();
            // Return extended vertsion of the message. 
            return message; 
        }

        string hide_urls(string message, int len_message)
        {
            string possible_url;
            int position_possible_url;
            for(int i =0; i < len_message; i++)
            {
                possible_url = "";
                // Start index of the possible URL; 
                position_possible_url = i;
                

                while(i< len_message && message[i] >= 'a' && message[i] <= 'z')
                {
                    possible_url += message[i];
                    i += 1; 
                }
                // We are possibly at the of the url; 
                if (string.IsNullOrWhiteSpace(possible_url))
                    continue;
                else
                {
                    // Check if the current substring is a string
                    if (!IsHttpUrl(possible_url))
                        continue;
                    else
                    {
                        // Remove the URL string and substitute it with “<URL Quarantined>";
                        // Substitute substring from position_possible_url to i :  URL....
                        //ReplaceAt(int index, int length, string replace)
                        int len_url = possible_url.Length;
                        message = message.Remove(position_possible_url, len_url).Insert(position_possible_url, "<URL Quarantined>");
                    }
                }
            }
            return message; 
        }

        /*
         * IsHttpUrl(string:: url) return true if the string provided in input is a link; 
         */
        private bool IsHttpUrl(string url)
        {
            return ((!string.IsNullOrWhiteSpace(url)) && (url.ToLower().StartsWith("http")));
        }


        /*
         * store_data(header, message, category of the message): store data in json file; 
         */
        void store_data(string header, string message, string category)
        {
            var dynObject = new { header = header, message = message, category = category};
            string JSONresult = JsonConvert.SerializeObject(dynObject);
            using (var tw = new StreamWriter(@"../../../"+path_storage_messages, true))
            {
                tw.WriteLine(JSONresult.ToString());
                tw.Close();
            }
        }


        /*
         *   Button_Send_Click(): used to validate the input received
         *   and store them if necessary
         * 
         */
        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            // store_data("header prova", "messaf", "cia");
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
                // Understand message type: Twitte, message, email, NONE (not indified)
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
                // Manage an email 
                // Call function to extend abbreviation
                string extended_message = extend_any_abbreviation(message, len_message);
                // Manage link presence
                extended_message = hide_urls(message, len_message);

                if (message_type == 1)
                    store_data(header, message, "message");

                // We are Managin an email
                else if (message_type == 2)
                {

                    // Identify message type: standard - significan Incident Reports

                    // Output message (extendend version) and store it in the json file. 
                    MessageBox.Show(extended_message);
                }
                else if(message_type == 3)
                {
                    store_data(header, message, "twitter");
                }

            }
        }

        /*
         * Button_Clear_Click(): remove any content insered in the message form
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
        }
    }
}
