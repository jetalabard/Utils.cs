using SlackAPI;
using SlackAPI.WebSocketMessages;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;
using Utils.FileReaderWriter.Serialization.StandardSerializer;

namespace SlackTools.Test
{
    public class SlackManager
    {
        private readonly string _Token;

        private readonly Encoding _encoding = new UTF8Encoding();

        private readonly SlackSocketClient _Client;

        private readonly List<string> _MessageReceive;

        delegate string MessageReceived(NewMessage message);
        

        public SlackManager(string token)
        {
            _Token = token;
            _Client = getClient();
            _MessageReceive = new List<string>();
        }


        public List<SlackMessage> Messages(string channelName)
        {
            using (WebClient client = new WebClient())
            {
                Channel channel = getChannel(channelName);
                var response = client.UploadValues("https://slack.com/api/channels.history", "POST",
                    new NameValueCollection() { { "token", _Token }, { "channel", channel.id } });
                //The response text is usually "ok"
                string responseText = _encoding.GetString(response);
                MessageResponse responseObj = new JsonSerializer<MessageResponse>().Deserialize(responseText);
                return responseObj.messages;
                //if responseText : error invalid_auth => token invalid
                //if responseText : error channel_not_found => channel invalid

            }

        }


        public SlackSocketClient getClient()
        {
            ManualResetEventSlim clientReady = new ManualResetEventSlim(false);
            SlackSocketClient client = new SlackSocketClient(_Token);
            client.Connect((connected) => {
                // This is called once the client has emitted the RTM start command
                clientReady.Set();
            }, () => {
                // This is called once the RTM client has connected to the end point
            });
            client.OnMessageReceived += (message) =>
            {
                // Handle each message as you receive them
                _MessageReceive.Add(message.text);
            };
            clientReady.Wait();
            return client;
        }



       
        public void sendChannelMessage(string message, string channelName = "general")
        {
            Channel channel = getChannel(channelName);
            _Client.PostMessage(null, channel.id, message);
        }


        public void sendDirectMessage(string message, string nameUser = "slackbot")
        {
            DirectMessageConversation dmchannel = DirectMessagesByUser(nameUser);
            _Client.PostMessage(null, dmchannel.id, message);
        }


        private User getUser(string userName)
        {
            User user = _Client.Users.Find(x => x.name.Equals(userName));
            if (user == null)
            {
                throw new ArgumentException("Invalid user name");
            }
            return user;
        }

        private Channel getChannel( string channelName)
        {
            Channel channel =  _Client.Channels.Find(c => c.name.Equals(channelName));
           if(channel == null)
            {
                throw new ArgumentException("Invalid channel name");
            }
            return channel;
        }


        public string getLastMessageDirect(string userName = "slackbot")
        {
            return DirectMessagesByUser(userName).latest.text;
        }


        private DirectMessageConversation DirectMessagesByUser(string user)
        {
            return _Client.DirectMessages.Find(x => x.user.Equals(getUser(user).id));
        }



        public ICollection<Channel> Channels()
        {
            return _Client.Channels;
        }


        public ICollection<User> Users()
        {
            ICollection<User> users = new List<User>();
            _Client.GetUserList((ulr) => {
                foreach(User user in ulr.members)
                {
                    users.Add(user);
                }
            });
            return users;
        }


     

    }
}
