using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibraryMessage
{
    public class TestClass
    {
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
        public string GetEmailSender(string message, int len_message)
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


        private bool isInputEmpty(string sender, string message)
        {
            if (string.IsNullOrWhiteSpace(sender) || string.IsNullOrWhiteSpace(message))
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
        public char get_message_nature(string header)
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

        /*
         * HideUrls(message, len_message)
         *      return the message without explicit urls address, storing each url in a list URLS
         *      and hiding the url in the message, writing instead <URL Quarantined>
         *      
         *      Returns the message without urls. 
         */
        public string HideUrls(string message, int len_message)
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
                    if (!IsHttpUrl(possible_url))
                        continue;
                    else
                    {
                        message_without_link = message.Replace(possible_url, "<URL Quarantined>");
                    }
                }
            }
            return message_without_link;
        }

        /*
         * IsHttpUrl(string:: url) return true if the string provided in input is a link; 
         */
        public bool IsHttpUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;
            if (url.ToLower().StartsWith("http"))
            {
                return true;
            }

            else
                return false;
        }

        /*
         *  GetTwitterUserId(message, len_message):
         *      return a twitterUserID it has been found. A twitter id is any string with lenght >2 that 
         *      starts with '@';
         *      "NO TWITTER USER ID FOUND" otherwise. 
         */
        public string GetTwitterUserID(string message, int len_message)
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
            return "NO TWITTER USER ID FOUND";
        }

        /*
         * IsPhoneNumber(number): 
         *      returns true if the given string is a mobile phone number. 
         *      false otherwise
         */
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
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
    }
}
