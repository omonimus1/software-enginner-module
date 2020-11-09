﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataLayer
{
    public class ManageData
    {
        private string path_storage_messages = "../../../DataLayer/data.json";

        /*
         * SerializeMessage(header, message, category of the message): store data in json file; 
        */
        public void SerializeMessage(string header, string sender, string message, char category, ref List<string> urls, ref Dictionary<string, int> hashtag)
        {
            // Print valided and categorised data to user before store them in json; 

            // Add also LIST OF string, store them and then clear them out; (if message is a twitter) 
            // store list of hashtag, store list of urls otherwise; 
            var dynObject = new
            {
                header = header,
                message = message,
                category = category,
                sender = sender,
                hashtag_list = hashtag,
                urls_list = urls
            };

            string JSONresult = JsonConvert.SerializeObject(dynObject);
            using (var tw = new StreamWriter(@"" + path_storage_messages, true))
            {
                tw.WriteLine(JSONresult.ToString());
                tw.Close();
            }
            // check which categoruy is it, and then do the clening
            urls.Clear();
            hashtag.Clear();
        }
    }
}