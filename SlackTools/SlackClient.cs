using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SlackTools
{
    public class SlackClient
    {
        private readonly Encoding _encoding = new UTF8Encoding();

        private readonly Uri _webhookUrl;
        private readonly HttpClient _httpClient = new HttpClient();

        //https://api.slack.com/tutorials/slack-apps-hello-world
        //https://gist.github.com/jogleasonjr/7121367

        public SlackClient(string urlWithAccessToken)
        {
            _webhookUrl = new Uri(urlWithAccessToken);
        }

        /// <summary>
        /// Post a message using simple strings
        /// </summary>
        /// <param name="text"></param>
        /// <param name="username"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool PostMessage(string text, string username = null, string channel = null)
        {
            Payload payload = new Payload(
                channel,
                username,
                text
            );

            return PostMessage(payload);
        }



       

        /// <summary>
        /// Post a message using a Payload object
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public bool PostMessage(Payload payload)
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["payload"] = payloadJson;

                var response = client.UploadValues(_webhookUrl, "POST", data);
                return _encoding.GetString(response) == "ok";
            }
        }

        /// <summary>
        ///  Post a async message using simple strings
        ///  we are sure that message is send or not
        /// </summary>
        /// <param name="urlWithAccessToken"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task IntegrateWithSlackAsync(string urlWithAccessToken, string message)
        {
            SlackClient slackClient = new SlackClient(urlWithAccessToken);
            HttpResponseMessage response = await slackClient.SendMessageAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Error to send async message :" + message);
            }
        }

        private async Task<HttpResponseMessage> SendMessageAsync(string message, string channel = null, string username = null)
        {
            Payload payload = new Payload(
                channel,
                username,
                message
            );
            var serializedPayload = JsonConvert.SerializeObject(payload);
            var response = await _httpClient.PostAsync(_webhookUrl,
                new StringContent(serializedPayload, Encoding.UTF8, "application/json"));

            return response;
        }


       
    }

}
