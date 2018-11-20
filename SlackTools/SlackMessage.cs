using System.Collections.Generic;
using System.Runtime.Serialization;
using Utils;
using Utils.FileReaderWriter.Serialization;

namespace SlackTools
{
    [DataContract(Name = "messageSlack", Namespace = "")]
    [KnownType(typeof(string))]
    public class SlackMessage : Serializable
    {

        /// <summary>
        /// text of message
        /// </summary>
        [DataMember]
        public string text
        {
            get; private set;
        }

        /// <summary>
        /// bot_id of message
        /// </summary>
        [DataMember]
        public string bot_id
        {
            get;private set;
        }

        [DataMember]
        public string type
        {
            get; private set;
        }

        [DataMember]
        public string subtype
        {
            get; private set;
        }

        [DataMember]
        public string ts
        {
            get; private set;
        }


        /// <summary>
        ///  constructor which define all properties
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="BotId"></param>
        /// <param name="Type"></param>
        /// <param name="Subtype"></param>
        /// <param name="Ts"></param>
        public SlackMessage(string Text, string BotId, string Type, string Subtype, string Ts)
            : base(new Dictionary<string, object>() { { "text", Text }, { "bot_id", BotId }, { "type", Type }, { "subtype", Subtype }, { "ts", Ts } })
        {
            //call construct method in parent class
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="elements"></param>
        public SlackMessage(StringList elements) : this(elements[0], elements[1], elements[2], elements[3], elements[4])
        {
        }

        /// <summary>
        /// construct user from list of elements
        /// </summary>
        public override void Construct()
        {
            Dictionary<string, object> elements = getElements();
            text = elements["text"] as string;
            bot_id = elements["bot_id"] as string;
            type = elements["type"] as string;
            subtype = elements["subtype"] as string;
            ts = elements["ts"] as string;
        }

        /// <summary>
        /// useful for Default serializer
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return text;
        }

    }
}
