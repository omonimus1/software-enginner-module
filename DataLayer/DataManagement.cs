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

        public void SerializeTrendingList(Dictionary<string, int> trending_hashtag)
        {
            string json_string = JsonConvert.SerializeObject(trending_hashtag, Formatting.Indented);
            using (var tw = new StreamWriter(@"" + PATH_TO_TRENDING_HASHTAG_LIST, true))
            {
                tw.WriteLine(json_string.ToString());
                tw.Close();
            }
        }
        public void SerializeSirList(string sort_code , string nature)
        {
            var sir_list_object = new
            {
                sort_code = sort_code,
                nature_message = nature,
            };
            // Call function to serialize sirList
            SerializeInJson(sir_list_object, PATH_TO_SIR_LIST);

            // try 1 sec to print it
            //DeserializeSirList();
        }

        /*
         public void DeserializeSirList()
         {

             //string filepath = "../../json1.json";
             string result = string.Empty;
             using (StreamReader r = new StreamReader(PATH_TO_SIR_LIST))
             {
                 var json = r.ReadToEnd();
                 var jobj = JObject.Parse(PATH_TO_SIR_LIST);
                 foreach (var item in jobj.Properties())
                 {
                     item.Value = item.Value.ToString().Replace("v1", "v2");
                 }
                 result = jobj.ToString();
                 Console.WriteLine(result);
             } 
             // File.WriteAllText(filepath, result);
             // JSON is the string
             JObject parsed = JObject.Parse(PATH_TO_SIR_LIST);

             foreach (var pair in parsed)
             {
                 Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
             }
         }
             */

        public void SerializeInJson(object objectToSerialize, string filename)
        {
            string JSONresult = JsonConvert.SerializeObject(objectToSerialize);
            using (var tw = new StreamWriter(@"" + filename, true))
            {
                tw.WriteLine(JSONresult.ToString());
                tw.Close();
            }
        }
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
