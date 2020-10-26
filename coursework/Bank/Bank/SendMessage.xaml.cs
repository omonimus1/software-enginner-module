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
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

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
        Dictionary<string, int> hashtag = new Dictionary<string, int>();
        List<string> urls = new List<string>();

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
            if (string.IsNullOrWhiteSpace(sender) || string.IsNullOrWhiteSpace(message))
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
        char get_message_nature(string header)
        {
            if (header[0] == 'T')
                return 'T';
            else if (header[0] == 'E')
                return 'E';
            else if (header[0] == 'S')
                return 'S';
            // Return error message: the type of messager has not been
            else
                return 'N';
        }



        string extend_any_abbreviation(string message, int len_message)
        {
            // Check if abbreviation is inside the CVS file:
            StreamReader sr = new StreamReader(@"../../../" + path_abbreviation_list);
            string possible_abbreviation = "";
            string extended_abbreviation;
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
                        extended_abbreviation = _values[1];
                         MessageBox.Show(_values[1]);
                        // Here add the enxtation with this structure: <FullExtansion>
                        //extended_abbreviation = ;
                        //extended_abbreviation = extended_abbreviation.Insert(0, "<");
                        
                        // Add the entended appreviation at the position of where we found them 
                        message = message.Insert(i, _values[1]);
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
            var message_without_link = "";
            string possible_url;
            int position_possible_url;
            for (int i = 0; i < len_message; i++)
            {
                possible_url = "";
                // Start index of the possible URL; 
                position_possible_url = i;
                

                while (i < len_message && message[i] !=  ' ')
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
                        // int len_url = possible_url.Length;
                        urls.Add(possible_url);
                        message_without_link = message.Replace(possible_url, "<URL Quarantined>");
                        //var replacement = source.Replace("mountains", "peaks");
                    }
                    
                    message_without_link = Regex.Replace(message_without_link,
                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                "<a target='_blank' href='$1'>$1</a>"); 
                }
            }
            return message_without_link;   
        }

        /*
         * IsHttpUrl(string:: url) return true if the string provided in input is a link; 
         */
        private bool IsHttpUrl(string url)
        {
            if ( string.IsNullOrWhiteSpace(url))
                return false;
            if (url.ToLower().StartsWith("http"))
            {
                MessageBox.Show("We got a link");
                return true;
            }
                
            else
                return false; 
        }


        void PrintCategorisedData (string header, string message, char category)
        {
            // MessageBox.Show(oggetto.message);
            MessageBox.Show("Message ID: " + header + Environment.NewLine + "Message body: " + message + Environment.NewLine + "Category: " +
                category +
                Environment.NewLine + string.Join(Environment.NewLine, hashtag) + Environment.NewLine + string.Join(Environment.NewLine, urls)
            );
        }

        /*
         * store_data(header, message, category of the message): store data in json file; 
         */
        void store_data(string header, string message, char category)
        {
            // Print valided and categorised data to user before store them in json; 
            PrintCategorisedData(header, message, category);

            // Add also LIST OF string, store them and then clear them out; (if message is a twitter) 
            // store list of hashtag, store list of urls otherwise; 
            var dynObject = new { header = header, message = message, category = category, hashtag_list = hashtag, urls_list = urls };
            
            string JSONresult = JsonConvert.SerializeObject(dynObject);
            using (var tw = new StreamWriter(@"../../../" + path_storage_messages, true))
            {
                tw.WriteLine(JSONresult.ToString());
                tw.Close();
            }
            // check which categoruy is it, and then do the clening
            urls.Clear();
            hashtag.Clear();
        }

        bool IsValidEmail(string possible_email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(possible_email);
                return addr.Address == possible_email;
            }
            catch
            {
                return false;
            }
        }

        string GetEmailSender(string message, int len_message)
        {
            string word;
            // Iterate inside the message, check if any word is an email, if yes, return it. 
            for (int i = 0; i < len_message; i++)
            {
                word = "";
                while (i < len_message && message[i] != ' ')
                {
                    word += message[i];
                    i += 1;
                }
                if (!string.IsNullOrWhiteSpace(word))
                {
                    if (IsValidEmail(word))
                        return word;
                }
            }
            // No EMAIL Id has been found; 
            return "NOT EMAIL ID FOUND";
        }

        string GetTwitterUserID(string message, int len_message)
        {
            string twitter_id;
            // Iterate inside the message, check if any word is an email, if yes, return it. 
            for (int i = 0; i < len_message; i++)
            {
                twitter_id = "";
                while (i < len_message && message[i] != ' ')
                {
                    twitter_id += message[i];
                    i += 1;
                }
                if (!string.IsNullOrWhiteSpace(twitter_id))
                {
                    if (twitter_id[0] == '@')
                        return twitter_id;
                }
            }
            // No EMAIL Id has been found; 
            return "NOT TWITTER USER ID FOUND";
        }

        void StoreListOfHashtag(string message, int len_message)
        {
            // Hashtag: word with a Lenght >= 2, where the first char is '#';
            string possible_hashtag;
            for (int i = 0; i < len_message; i++)
            {
                possible_hashtag = "";
                while (i < len_message && message[i] != ' ')
                {
                    possible_hashtag += message[i];
                    i += 1;
                }
                // If string is NOT null and is NOT  whitespace
                if (!string.IsNullOrWhiteSpace(possible_hashtag))
                {
                    // Check if string has Lenght >=2 and stars with '#';
                    if (possible_hashtag.Length >= 2 && possible_hashtag[0] == '#')
                    {
                        // Check if the key exists; 
                        if (hashtag.ContainsKey(possible_hashtag))
                            hashtag[possible_hashtag]++;
                        else
                            hashtag.Add(possible_hashtag, 1);
                    }
                        
                }
            }
        }


        /*
         *   Button_Send_Click(): used to validate the input received
         *   and store them if necessary
         */
        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            string header = txtBoxSender.Text;
            string message = txtBoxMessage.Text;

            string message_sender = "";
            if (isInputEmpty(header, message))
            {
                MessageBox.Show("Make sure you have filled sender and message textboxes", "Validation Error");
                return;
            }
            // Start validation process
            else
            {
                // Understand message type: Twitte, message, email, NONE (not indified)
                char message_type = get_message_nature(header);
                if(message_type == 'N')
                {
                    MessageBox.Show("Message nature has not be recognized");
                }
                int len_message = message.Length;
                if (message_type == 'E' && len_message > 1028 || message_type != 'E' && len_message > 140)
                {
                    MessageBox.Show("Your messsage is too long");
                    return;
                }
                // Check if message type has not been recognized. 
                if (message_type == 'N')
                {
                    // Print error message, and stop the execution of the function 
                    MessageBox.Show("The nature of the message that you have innserte, has now been recognized");
                }
                else if(message_type == 'S')
                {
                    // Extend abbreviatios 
                    message = extend_any_abbreviation(message, len_message);
                    // Store in json file 
                    store_data(header, message, message_type);
                }
                else if(message_type == 'E')
                {
                    // search for an email in the body message; 
                    sender = GetEmailSender(message, len_message);
                    // Call function to extend abbreviation
                    message = extend_any_abbreviation(message, len_message);
                    // Hide URLs and store them in a list and Store URLS in LIST
                    message  = hide_urls(message, len_message);
                    // Store in json file 
                    store_data(message_sender, message, message_type);
                }
                else if(message_type == 'T')
                {

                    // Extend messager abbreviation; 
                    // Extend messager abbreviation; 
                    message = extend_any_abbreviation(message, len_message);
                    
                    // search ID twitter user in the body
                    sender = GetTwitterUserID(message, len_message);
                    // search all hashtag and store them in a list;
                    StoreListOfHashtag(message, len_message);
 
                    // Store message in json file 
                    store_data(message_sender, message, message_type);
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

    }
}
