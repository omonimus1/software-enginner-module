﻿using Newtonsoft.Json;
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

namespace BusinessLayer
{
{
    /// <summary>
    /// Interaction logic for SendMessage.xaml
    /// </summary>
    public partial class SendMessage : Window
    {
        public string TextMessage;
        // Path to the cvs file that contains all the possible abbreviations 
        // and their extented meaning
        //private  string PATH_ABBREVIATION_LIST = "../../../DataLayer/textwords.csv";
        string abbreviation_list = "textwords.csv";
        private const int MAX_LENGTH_TWITTER_ID = 16;

        Dictionary<string, int> hashtag = new Dictionary<string, int>();
        List<string> urls = new List<string>();

        // Existing Category of important email 
        string[] urgent_email_categories = { "theft", "staff attack", "ATM theft", "raid", "customer attack", "staff abuse", "bom threat", "terrorism",
            "suspicious incident", "intelligence", "cash loss"};

        /*
         * TextBoxMessage_TextChange(): Will keep the form open while user is typing
         */
        public void TextBoxMessage_TextChanged(object sender, EventArgs e)
        {
            this.TextMessage = txtBoxMessage.Text;
        }

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

        /*
         * get_message_nature(): return the nature of the message in base header
         * - Return  S: if the header is of a mobile phone 
         * - Return  E: if the header is an email address
         * - Return  T: if the header is from a twitter user
         * - Return  N: if the header type has not been recognised
         */
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




        /*
         * extend_any_abbreviation(message, len_message):
         *      It search on textwords.csv file any possible abbreaviation and add to the message
         *      body the meaning of this abbreaviation. 
         *      It returns the extended version of the message. 
         */
        /*
  * extend_any_abbreviation(message, len_message):
  *      It search on textwords.csv file any possible abbreaviation and add to the message
  *      body the meaning of this abbreaviation. 
  *      It returns the extended version of the message. 
  */
        string ExtendAbbreviationInsideMessage(string message, int len_message)
        {
            // Process storage and check if abbreviations needs to be extended
            // Process storage and check if abbreviations needs to be extended
            /* “Saw your message ROFL can’t wait to see you” becomes “Saw your message 
             * ROFL<Rolls on the floor laughing> can’t wait to see you” */

            // Expand abbreviations
            // path_abbreviation_list
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
                // If abbreviation is empty skype
                if (possible_abbreviation == "" || possible_abbreviation == " ")
                    continue;
                // Check if abbreviation is inside the CVS file:
                StreamReader sr = new StreamReader(@"../../../DataLayer/" + abbreviation_list);
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
                        string extended_abbreviation = " <" + _values[1] + "> ";
                        // Add the entended appreviation at the position of where we found them 
                        message = message.Insert(i, extended_abbreviation);
                        return message;
                    }
                }
            }
            // Return extended vertsion of the message. 
            return message;
        }

        /*
         * HideUrls(message, len_message)
         *      return the message without explicit urls address, storing each url in a list URLS
         *      and hiding the url in the message, writing instead <URL Quarantined>
         *      
         *      Returns the message without urls. 
         */
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

        /*
         * IsHttpUrl(string:: url) return true if the string provided in input is a link; 
         */
        public bool IsHttpUrl(string possible_url)
        {

            Uri uriResult;
            bool result = Uri.TryCreate(possible_url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        public bool IsIncidentReportEmail(string subject)
        {
            // Search the subject of the email has a prority keyword  /* or a bit longer: (stringArray.Any(s => stringToCheck.Contains(s))) */
            if (urgent_email_categories.Any(subject.Contains))
                return true;
            else
                return false;
        }

        /*
         * PrintCategorisedData (header, sender, message, category)
         *  Output the message ID, message body, category, sender and list of hashtag and urls; 
         */
        void PrintCategorisedData(string header, string sender, string message, string subject, char category)
        {
            if (category == 'S')
            {
                MessageBox.Show("Message ID: " + header
                     + Environment.NewLine
                     + "Category:⦁SMS Message"
                     + Environment.NewLine
                     + "Mobile Phone Number Sender: " + sender
                     + Environment.NewLine
                     + "Message body: " + message
                     + Environment.NewLine
                     + string.Join(Environment.NewLine, hashtag)
                     + Environment.NewLine
                     + string.Join(Environment.NewLine, urls)
                 );
            }
            else if (category == 'E')
            {
                MessageBox.Show("Message ID: " + header
                    + Environment.NewLine
                    + "Category:⦁Email Messages"
                    + Environment.NewLine
                    + "Sender email: " + sender
                    + Environment.NewLine
                    + "Subject: " + subject
                    + Environment.NewLine
                    + "Message body: " + message
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
                 + "Message body: " + message
                 + Environment.NewLine
                 + string.Join(Environment.NewLine, hashtag)
                 + Environment.NewLine
                 + string.Join(Environment.NewLine, urls)
             );
            }

        }
        /*
    * IsValidEmail(possible email): returns true if a given string respect the email format
    */
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

        /*
   *  GetTwitterUserId(message, len_message):
   *      return a twitterUserID it has been found. A twitter id is any string with lenght >2 that 
   *      starts with '@';
   *      "NO TWITTER USER ID FOUND" otherwise. 
   */
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

        /*
         * IsPhoneNumber(number): 
         *      returns true if the given string is a mobile phone number. 
         *      false otherwise
         */
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+44\s?7\d{3}|\(?07\d{3}\)?)\s?\d{3}\s?\d{3}$", RegexOptions.IgnoreCase).Success;

        }

        /*
         * GetMobilePhoneSender(message, len_message):
         *      search inside a string a mobile phone number; 
         */
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

        /*
         * StoreListOfHashtag(message, len_message):
         *      search inside a message an hastage and store any of them 
         *      in the hashta lists.
         */
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
                    }
                }
            }
        }

        public void ManageMessage(string message)
        {
            DataManagement m = new DataManagement();
            string sender_ = "Sender unkown";
            string subject = "";
            string priority_email;
            // Understand message type: Twitte, message, email, NONE (not indified)
            string message_id = GetMessageId(message);
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
                m.SerializeMessage(message_id, sender_, message, subject, message_id[0], "", ref urls, ref hashtag);
                PrintCategorisedData(message_id, sender_, message, subject, message_id[0]);
            }
            else if (message_id[0] == 'E')
            {

                int end_email_index = 0;
                sender_ = GetEmailSender(message, len_message, ref end_email_index);
                // Get email subject(20 chars after email sender);

                int i = 0;
                while (i < 20 && end_email_index < len_message)
                {
                    subject += message[end_email_index];
                    end_email_index += 1;
                    i += 1;
                }
                bool incident = IsIncidentReportEmail(subject);
                if (incident == true)
                    priority_email = "Incident Report";
                else
                    priority_email = "Regular Message";
                // Call function to extend abbreviation
                message = ExtendAbbreviationInsideMessage(message, len_message);
                // Hide URLs and store them in a list and Store URLS in LIST
                message = HideUrls(message, len_message);
                PrintCategorisedData(message_id, sender_, message, subject, message_id[0]);

                // Store in json file 
                m.SerializeMessage(message_id, sender_, message, subject, message_id[0], priority_email, ref urls, ref hashtag);
            }
            else if (message_id[0] == 'T')
            {

                // Extend messager abbreviation; 
                // Extend messager abbreviation; 
                message = ExtendAbbreviationInsideMessage(message, len_message);

                // search ID twitter user in the body
                sender_ = GetTwitterUserID(message, len_message);
                // search all hashtag and store them in a list;
                StoreListOfHashtag(message, len_message);
                PrintCategorisedData(message_id, sender_, message, subject, message_id[0]);

                // Store message in json file 
                m.SerializeMessage(message_id, sender_, message, subject, message_id[0], "", ref urls, ref hashtag);
            }
        }
        /*
         *   Button_Send_Click(): used to validate the input received
         *   and store them if necessary
         */
        public void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            string message = txtBoxMessage.Text;


            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Make sure you have filled sender and message textboxes", "Validation Error");
                return;
            }
            else
            {
                // Startup validaion message process
                ManageMessage(message);
            }
        }

        /*
         * Button_Clear_Click(): remove any content insered in the message form
         */
        public void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtBoxMessage.Text = "";
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        /*
         * GetEmailSender(message, len_message):
         *      Search inside the message string an email and returs it. 
         *      If there are no email inside the text, it will return: "NOT EMAIL ID FOUND"
         */
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
