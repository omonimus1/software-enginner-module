using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataLayer
{
    public class DataManagement
    {
        private const string PATH_STORAGE_MESSAGES = "../../../DataLayer/data.json";
        private const string PATH_TO_SIR_LIST = "../../../DataLayer/sir_list.json";
        private const string PATH_TO_TRENDING_HASHTAG_LIST = "../../../DataLayer/global_hashtag.json";

        /// <summary>
        ///     Load the hashtable used to keep tradck of each hashtag and its frequency 
        /// </summary>
        /// <returns>Hashmap containing all hashtags and each frequencies. </returns>

        public Dictionary<string, int> LoadTrendingHashtagList()
        {
            // Read json content and store it in a string
            Dictionary<string, int> hashtag_trending_list;
            string json;
            using (StreamReader r = new StreamReader(PATH_TO_TRENDING_HASHTAG_LIST))
            {
                json = r.ReadToEnd();
            }
            hashtag_trending_list = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
            return hashtag_trending_list;
        }


        /// <summary>
        ///     Serialize the sort_code and nature of any email.
        ///     The email could be a regular or emergency one.
        /// </summary>
        /// <param name="trending_hashtage">Updated app global hashtag map,containing updated frequencies of each hashtag</param>
        public void SerializeTrendingList(Dictionary<string, int> trending_hashtag)
        {
            // Erase content of the trending hashtag file
            System.IO.File.WriteAllText(@""+PATH_TO_TRENDING_HASHTAG_LIST, string.Empty);
            string json_string = JsonConvert.SerializeObject(trending_hashtag, Formatting.Indented);
            using (var tw = new StreamWriter(@"" + PATH_TO_TRENDING_HASHTAG_LIST, true))
            {
                tw.WriteLine(json_string.ToString());
                tw.Close();
            }
        }

        /// <summary>
        ///     Serialize the sort_code and nature of any email.
        ///     The email could be a regular or emergency email.
        /// </summary>
        /// <param name="sort_code">Bank account sort-code present inside the email subject</param>
        /// <param name="nature">Nature of the message: Non emergency or emergency category; Ex: intelligence, cash loss</param>
        public void SerializeSirList(string sort_code , string nature)
        {
            var sir_list_object = new
            {
                sort_code = sort_code,
                nature_message = nature,
            };
            // Call function to serialize sirList
            SerializeInJson(sir_list_object, PATH_TO_SIR_LIST);
        }

        /// <summary>
        ///     LoadSirList(): Read and store the content of the sir_list.json file in a string; 
        /// </summary>
        /// <returns>content of the sir_list.json file</returns>
        public string LoadSirList()
         {
            // Read json content and store it in a string
            string json;
            using (StreamReader r = new StreamReader(PATH_TO_SIR_LIST))
            {
                json = r.ReadToEnd();
            }
            //json = JsonConvert.DeserializeObject<string>>(json);
            return json;

        }

        /// <summary>
        ///     LoadSirList(): Read and store the content of the sir_list.json file in a string; 
        /// </summary>
        /// <param name="filename">filename where to serialize the object</param>
        /// <param name="objectToSerialize">Object to serialize</param>
        public void SerializeInJson(object objectToSerialize, string filename)
        {
            string JSONresult = JsonConvert.SerializeObject(objectToSerialize);
            using (var tw = new StreamWriter(@"" + filename, true))
            {
                tw.WriteLine(JSONresult.ToString());
                tw.Close();
            }
        }

        /// <summary>
        ///     Serialize message detailes
        /// </summary>
        /// <param name="header">Header of the mesage</param>
        /// <param name="sender">Sender id: it can be represented by an email, mobile phone or tweet ID</param>
        /// <param name="message">Message body</param>
        /// <param name="subject">subject of the message</param>
        /// <param name="category">Cateogry of the message: E: email - S: text message - t: Tweet</param>
        /// <param name="priority_email">Priority of the email: it can be null or indicate a speicifc category: cash less / intelligence etc. </param> 
        /// <param name="url">List of urls quarantined present inside the email</param> 
        /// <param name="hashtag">List of hashtag and  hashtag frequencies present inside the tweet message</param> 
        public void SerializeMessage(string header, string sender, string message, string subject, char category, string priority_email, ref List<String> urls, ref Dictionary<string, int> hashtag)
        { 
            // Add also LIST OF string, store them and then clear them out; (if message is a twitter) 
            // store list of hashtag, store list of urls otherwise; 
            var dynObject = new
            {
                message_id = header,
                category = category,
                subject = subject,
                message = message,
                sender = sender,
                hashtag_list = hashtag,
                urls_list = urls
            };
            SerializeInJson(dynObject, PATH_STORAGE_MESSAGES);
        }
    }
}
