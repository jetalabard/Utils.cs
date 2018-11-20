using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlackTools
{
    //This class serializes into the Json payload required by Slack Incoming WebHooks
    public class Payload
    {
        public Payload(string channel, string username, string text)
        {
            Text = text;
            Channel = channel;
            Username = username;
        }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
