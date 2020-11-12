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
        private const string PATH_TO_TRENDING_HASHTAG_LIST = "../../../DataLayer/glabal_hashtag.json";

        public void SerializeTrendingList(Dictionary<string, int> dic)
        {

        }
        public void SerializeSirList(string sort_code , string nature)
        {
            var sir_list_object = new
            {
                sort_code = sort_code,
                nature = nature,
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
            urls.Clear();
            hashtag.Clear();     
        }
    }
}
