using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataLayer;
using System.Collections.Specialized;
using System.Globalization;

namespace Coursework2
{
    /// <summary>
    /// Interaction logic for SendMessage.xaml
    /// </summary>
    public partial class SendMessage : Window
    {
        public SendMessage()
        {
            InitializeComponent();
        }
        DataManagement data = new DataManagement();

        public string TextMessage;
        // Path to the cvs file that contains all the possible abbreviations 
        // and their extented meaning
        private const string ABBREVIATION_LIST_FILENAME = "textwords.csv";
        private const int MAX_LENGTH_TWITTER_ID = 16;


        Dictionary<string, int> global_hashtag = new Dictionary<string, int>();
        Dictionary<string, int> hashtag = new Dictionary<string, int>();
        List<string> urls = new List<string>();

        // Existing Category of important email 
        string[] urgent_email_categories = { "theft", "staff attack", "ATM theft", "raid", "customer attack", "staff abuse", "bom threat", "tNot Foundism",
            "suspicious incident", "intelligence", "cash loss"};

        public void TextBoxMessage_TextChanged(object sender, EventArgs e)
        {
            this.TextMessage = txtBoxMessage.Text;
        }

        private void TextBoxSubject_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        ///     Check if a given string is an UK mobile phon number: for example 07464329999
        /// </summary>
        /// <param name="possible_id">String to analize</param>
        /// <returns>True if the given string is an uk mobile phone number, false otherwise</returns>
        public bool IsAMessageID(string possible_id)
        {
            // Check if from index from the second char to the last, all chars are digits; 
            for (int i = 1; i < possible_id.Length; i++)
            {
                if (!char.IsDigit(possible_id[i]))
                    return false;
            }
            if (possible_id[0] == 'T' || possible_id[0] == 'E' || possible_id[0] == 'S')
                return true;
            else
                return false;
        }

        /// <summary>
        ///     Analize the message subject and extract the message cateogy by analizing the ID contained in the 
        ///     subject
        /// </summary>
        /// <returns>S: if the message is a text-message, T if the mesage is a tweet, E if it's an email, N otherwise
        /// if the message nature has not been recognised</returns>
        public string GetMessageId(string header)
        {

            int len_header = header.Length;
            if (len_header < 10)
                return "N";

            string possible_message_id;
            // Iterate inside the header to search the ID; 
            for (int i = 0; i < len_header; i++)
            {
                possible_message_id = "";
                while (i < len_header && header[i] != ' ')
                {
                    possible_message_id += header[i];
                    i += 1;
                }
                if (possible_message_id.Length != 10)
                    continue;
                else
                {
                    if (IsAMessageID(possible_message_id))
                        return possible_message_id;
                }
            }
            // No message ID found; 
            return "N";
        }

        /// <summary>
        ///    Given a message, it analize each string of the message and check if any of these
        ///    string is an text abbreviation stored in a .csv with the extention mearning. 
        ///    If an abbreavetion is found, the meaning of the abbreavitation will be added in the message; 
        /// </summary>
        /// <param name="message">Message to analize</param>
        /// <param name="len_message">Length of the message</param>
        /// <returns>Message with the meaning of each text abbreviation</returns>
        public string ExtendAbbreviationInsideMessage(string message, int len_message)
        {
            string possible_abbreviation = "";
            for (int i = 0; i < len_message; i++)
            {
                possible_abbreviation = "";
                while (i < len_message && message[i] == '/' || i < len_message && message[i] >= 'A' && message[i] <= 'Z')
                {
                    possible_abbreviation += message[i];
                    i += 1;

                }
                // Now, we have the possible abbreviation; 
                // If abbreviation is empty skip
                if (possible_abbreviation == "" || possible_abbreviation == " ")
                    continue;
                // Check if abbreviation is inside the CVS file:
                StreamReader sr = new StreamReader(@"../../../DataLayer/" + ABBREVIATION_LIST_FILENAME);
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
                        //MessageBox.Show(_values[1]);
                        // Here add the enxtation with this structure: <FullExtansion>
                        string extended_abbreviation = " <" + _values[1] + ">";
                        // Add the entended appreviation at the position of where we found them 
                        message = message.Insert(i, extended_abbreviation);
                        return message;
                    }
                }
            }
            // Return extended vertsion of the message. 
            return message;
        }

        /// <summary>
        ///      Hide any url contained in the message body, substituting these urls with the string: "<QUARANTINED>"
        /// </summary>
        /// <param name="message">Messge body</param>
        /// <param name="len_message">Length of the message</param>
        /// <returns>Returns the message without any urls.</returns>
        string HideUrls(string message, int len_message)
        {
            var message_without_link = "";
            string possible_url;
            int position_possible_url;
            for (int i = 0; i < len_message; i++)
            {
                possible_url = "";
                // Start index of the possible URL; 
                position_possible_url = i;

                while (i < len_message && message[i] != ' ')
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
                    if (IsHttpUrl(possible_url))
                    {
                        urls.Add(possible_url);
                        message_without_link = message.Replace(possible_url, "<URL Quarantined>");
                    }
                }
            }
            return message_without_link;
        }

        /// <summary>
        ///     Check if a given string is an http/https/ftp url
        /// </summary>
        /// <param name="possible_url">String to analize</param>
        /// <returns>True if possible_url is a valid url, false otherwise</returns>
        public bool IsHttpUrl(string possible_url)
        {

            Uri uriResult;
            bool result = Uri.TryCreate(possible_url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        /// <summary>
        ///     Check if any keyword of the subject matches a list of special set of words
        ///     used to report an emergency situation; 
        /// </summary>
        /// <param name="subject">List of string present in the subject</param>
        /// <returns>True if the subject contains </returns>
        public bool IsIncidentReportEmail(String[] subject)
        {
            // Search the subject of the email has a prority keyword  /* or a bit longer: (stringArray.Any(s => stringToCheck.Contains(s))) */
            if (urgent_email_categories.Any(subject.Contains))
                return true;
            else
                return false;
        }

        /// <summary>
        ///     Output the content of the sir_list.json file
        /// </summary>
        void PrintContentSirList()
        {
            // Load SirList Content
            string sir_list_content = data.LoadSirList();
            // Print sir list
           MessageBox.Show(sir_list_content, "SIR LIST");
            // Clear content of the sir list
        }

        /// <summary>
        ///     Print the updated global list of hashtag; 
        /// </summary>
        void PrintAndEraseGlobalHashtag()
        {
            MessageBox.Show(string.Join(Environment.NewLine, global_hashtag), "Trend Hashtag List");
            // global_hashtag.Clear();
        }

        /// <summary>
        ///      Output details of the message sent
        /// </summary>
        /// <param name="header">header of the message</param>
        /// <param name="sender">Sender ID: mobile phone - email - tweeter user id</param>
        /// <param name="message">Body message</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="category">Cateogory: Unkown / Text message / tweet </param>
        /// <param name="emergency_nature">Egerncy nature: none / iuntellignece / cash loss / back found etc..</param>
        void PrintCategorisedData(string header, string sender, string message, string subject, char category, string emergency_nature)
        {
            if (category == 'S')
            {
                MessageBox.Show("Message ID: " + header
                     + Environment.NewLine
                     + "Category:⦁SMS Message"
                     + Environment.NewLine
                     + "Mobile Phone Number Sender: " + sender
                     + Environment.NewLine
                     + "Full Message: " + message
                 );
            }
            else if (category == 'E')
            {
                MessageBox.Show("Message ID: " + header
                    + Environment.NewLine
                    + "Category:⦁Email Messages"
                    + Environment.NewLine
                    + "Emergecy Nature: " + emergency_nature
                    + Environment.NewLine
                    + "Sender email: " + sender
                    + Environment.NewLine
                    + "Subject: " + subject
                    + Environment.NewLine
                    + "Full Message:" + message
                    + Environment.NewLine
                    + "List of URLS found in the message"
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, urls)
                );
            }
            else if (category == 'T')
            {
                MessageBox.Show("Message ID: " + header
                 + Environment.NewLine
                 + "Category:⦁Tweet"
                 + Environment.NewLine
                 + "Twitter user Id: " + sender
                 + Environment.NewLine
                 + "Full Message:" + message
                 + Environment.NewLine
                + "List of Hashtags and frequency found in the message:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, hashtag) 
                + Environment.NewLine
                + "List of URLS found in the message"
                + Environment.NewLine
                + string.Join(Environment.NewLine, urls)
             );
            }
            else
            {
                MessageBox.Show("Error: Nature of the message has not been recognised");
            }

        }


        /// <summary>
        ///     Check if a given string is an email
        /// </summary>
        /// <param name="possible_email">string to analize</param>
        /// <returns>True if the given string is an email, false otherwise</returns>
        public bool IsValidEmail(string possible_email)
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

   
        /// <summary>
        ///     Returns the firt occurrency of a tweet user id; 
        /// </summary>
        /// <param name="message">Mesage</param>
        /// <param name="len_message">Length of the message</param>
        /// <returns>Twitter user id if it exists, error message otherwise</returns>
        public string GetTwitterUserID(string message, int len_message)
        {
            const string TWEET_ID_NOT_FOUND = "Twttier User ID not found - Max Twitter ID Length allowed: 16";
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
                if (!string.IsNullOrWhiteSpace(twitter_id) && twitter_id.Length <= MAX_LENGTH_TWITTER_ID)
                {
                    if (twitter_id[0] == '@')
                        return twitter_id;
                }
            }
            // No EMAIL Id has been found; 
            return TWEET_ID_NOT_FOUND;
        }


        /// <summary>
        ///     Check if a given string is a mobile phone number
        ///     
        /// </summary>
        /// <param name="number">string to analize</param>
        /// <returns>True if the given string is a valid UK mobile phone number, false otherwise</returns>
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+44\s?7\d{3}|\(?07\d{3}\)?)\s?\d{3}\s?\d{3}$", RegexOptions.IgnoreCase).Success;

        }


        /// <summary>
        ///         Return a mobile phone number contained in the message if it exists
        /// </summary>
        /// <param name="message">message to analize</param>
        /// <param name="len_message">Length of the message</param>
        /// <returns> mobile phone number contained in the message if it exists, error message otherwise</returns>
        public string GetMobilePhoneSender(string message, int len_message)
        {
            string possible_number;
            for (int i = 0; i < len_message; i++)
            {
                possible_number = "";
                while (i < len_message && Char.IsDigit(message[i]) || i < len_message && message[i] == '+')
                {
                    possible_number += message[i];
                    i += 1;
                }
                if (!string.IsNullOrWhiteSpace(possible_number))
                {
                    if (IsPhoneNumber(possible_number))
                        return possible_number;
                }
            }
            return "Unkown mobile phone number";
        }

        /// <summary>
        ///     Create hashmap used to register the hashtag and frequency of each 
        ///     hashtag in the message; 
        /// </summary>
        /// <param name="message">Message body</param>
        /// <param name="len_message">Length of the message</param>
        public void StoreListOfHashtag(string message, int len_message)
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
                        // Check if the same hashtag is present in the GLOBAL trending list
                        if (global_hashtag.ContainsKey(possible_hashtag))
                            global_hashtag[possible_hashtag]++;
                        else
                            global_hashtag.Add(possible_hashtag, 1);
                    }
                }
            }
            // Serialize global hashtag trending list
            data.SerializeTrendingList(global_hashtag);
        }

        
        public bool IsSubjectIncidentReport(string subject)
        {
            Boolean hasDate = false;
          DateTime dateTime = new DateTime();
            String[] inputText = subject.Split(' ');//split on a whitespace

            //Use the Parse() method
            try
            {
                dateTime = DateTime.Parse(inputText[1]);
                hasDate = true;
                //break;//no need to execute/loop further if you have your date
                if (hasDate == true && inputText[0] == "SIR")
                    return true;
                else
                {
                    return false; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Not Found while checking if email is priority Email");
                return false; 
            }
        }

        /// <summary>
        ///     Check if a given string is a UK bank acccount sort-code
        /// </summary>
        /// <param name="possible_sort_code">Possible sort code to validate</param>
        /// <returns>Returns true if the given string is  avalid sort-code, false otherwise</returns>
        bool IsSortCode(string possible_sort_code)
        {
            Regex r = new Regex(@"\b[0-9]{2}-?[0-9]{2}-?[0-9]{2}\b");
            Match match1 = r.Match(possible_sort_code);
            if (match1.Success)
            {
                return true; 
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///     Check if a given string is a UK bank account sort code or not; 
        /// </summary>
        /// <param name="inputText">list of string that composes the message</param>
        /// <returns>Valid sort code if exists, error message otherwise</returns>
        string GetSortCode(String[] inputText)
        {
            foreach (string word in inputText)
            {
                if (IsSortCode(word))
                    return word; 
            }
            return "Sort Code Not Found"; 
        }


        /// <summary>
        ///     Searh inside the message at least one emergency keyword used to categorize the nature of the message
        /// </summary>
        /// <param name="message_array">Given string to analize</param>
        /// <returns>Nature of the incident report - error message otherwise</returns>
        string GetNatureIncendent(String[] message_array)
        {
            int len = message_array.Length;
            for (int i = 0; i < len; i++)
            {
                if (Array.Exists(urgent_email_categories, element => element == message_array[i]))
                    return message_array[i];
            }
            // No mathes found
            return "Not Found";
        }

        /// <summary>
        ///         Used to manage different message elaboration in accordign to the 
        ///         nature of the message: text message / email / tweet
        /// </summary>
        /// <param name="message">Message to analize</param>
        /// <param name="subject">Subject of the message</param>
        public void ManageMessage(string message, string subject)
        {
            string sender_ = "Sender unkown";
            //string subject = "";
            string priority_email;
            // Understand message type: Twitte, message, email, NONE (not indified)
            string message_id = GetMessageId(subject);
            if (message_id == "N")
            {
                MessageBox.Show("Message nature has not be recognized");
            }
            int len_message = message.Length;
            if (message_id[0] == 'E' && len_message > 1028 || message_id[0] != 'E' && len_message > 156)
            {
                MessageBox.Show("Your messsage is too long");
                return;
            }
            else if (message_id[0] == 'S')
            {
                // Search of mobile phone number sender
                sender_ = GetMobilePhoneSender(message, len_message);

                // Extend abbreviatios 
                message = ExtendAbbreviationInsideMessage(message, len_message);
                // Store in json file 
                data.SerializeMessage(message_id, sender_, message, subject, message_id[0], "", ref urls, ref hashtag);
                PrintCategorisedData(message_id, sender_, message, subject, message_id[0], "none");
            }
            else if (message_id[0] == 'E')
            {
               
                int end_email_index = 0;
                sender_ = GetEmailSender(message, len_message, ref end_email_index);
                // Get email subject(20 chars after email sender);

                // Call function to extend abbreviation
                message = ExtendAbbreviationInsideMessage(message, len_message);
                // Hide URLs and store them in a list and Store URLS in LIST
                message = HideUrls(message, len_message);

                bool is_subject_of_a_incident_report = IsSubjectIncidentReport(subject);
                // Convert message in aray of string in according to the white space converter 
                String[] subject_array = subject.Split(' ');//split on a whitespace
                
                string sort_code = GetSortCode(subject_array);
                // Insert the string "Sort Code:" at the beginning of the bank sort code
                sort_code = sort_code.Insert(0, "Sort Code: ");
                string nature_of_incident = GetNatureIncendent(subject_array);
                bool incident = IsIncidentReportEmail(subject_array);
                
                
                if (nature_of_incident != "Not Found")
                {
                    priority_email = "Incident Report";
                    // Serialize subject, Sort code and Nature of incident
                    data.SerializeSirList(sort_code, nature_of_incident);
                    // Print All the content of the SIR List 

                }
                    
                else
                {
                    priority_email = "Regular Message";
                    nature_of_incident = "None - It's a regular message";
                }
                    
                // Print email details after string manipulation 
                PrintCategorisedData(message_id, sender_, message, subject, message_id[0], nature_of_incident);
                // Print all content of the sir_list.json file
                // Store in json file 
                data.SerializeMessage(message_id, sender_, message, subject, message_id[0], priority_email, ref urls, ref hashtag);
                PrintContentSirList();
            }
            else if (message_id[0] == 'T')
            {
                urls.Clear();
                hashtag.Clear();
                // Extend messager abbreviation; 
                message = ExtendAbbreviationInsideMessage(message, len_message);
                string total_message = message + subject; 
                // search ID twitter user in the body
                sender_ = GetTwitterUserID(total_message, len_message);

                // Load global hashtag trending list; 
                global_hashtag = data.LoadTrendingHashtagList();

                // search all hashtag and store them in a list;
                StoreListOfHashtag(total_message, len_message);
                PrintCategorisedData(message_id, sender_, message, subject, message_id[0], "none");

                // Store message in json file 
                data.SerializeMessage(message_id, sender_, message, subject, message_id[0], "", ref urls, ref hashtag);
                PrintAndEraseGlobalHashtag();
            }
        }
        /*
         *   Button_Send_Click(): used to validate the input received
         *   and store them if necessary
         */
        public void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            string message = txtBoxMessage.Text;
            string subject = txtBoxSubject.Text;
            message = message.Insert(0,subject);

            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(subject))
            {
                MessageBox.Show("Make sure you have filled sender and message textboxes", "Validation Error");
                return;
            }
            else
            {
                // Startup validaion message process
                ManageMessage(message, subject);
            }
        }

        /// <summary>
        ///     Empty both message and header textboxes used to insert manually the message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtBoxMessage.Text = "";
            txtBoxSubject.Text = "";
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        ///    Search inside the message string an email and returs it the email exists
        /// </summary>
        /// <param name="message">message to analize</param>
        /// <param name="len_message">Length of the message1</param>
        /// <param name="index_end_email">Rference parameter to keep track of the end of the email</param>
        /// <returns>Email if it exists, error message otherwise</returns>
        public string GetEmailSender(string message, int len_message, ref int index_end_email)
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
                    {
                        index_end_email = i;
                        return word;
                    }
                }
            }
            // No EMAIL Id has been found; 
            return "NOT EMAIL ID FOUND";
        }

    }
}
