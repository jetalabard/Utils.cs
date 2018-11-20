using System.Collections.Generic;
using System.Runtime.Serialization;
using Utils.FileReaderWriter.Serialization;

namespace SlackTools
{
    [DataContract(Name = "messageResponse", Namespace = "")]
    [KnownType(typeof(bool))]
    [KnownType(typeof(SlackMessage))]
    public class MessageResponse : Serializable
    {
        [DataMember]
        public bool ok;

        [DataMember]
        public List<SlackMessage> messages;

        [DataMember]
        public bool has_more;

        public MessageResponse(bool ok, List<SlackMessage> messages, bool has_more)
            : base(new Dictionary<string, object>() { { "ok", ok }, { "messages", messages }, { "has_more", has_more } })
        {
            //call construct method in parent class
        }

        public override void Construct()
        {
            Dictionary<string, object> elements = getElements();
            ok = (bool)elements["ok"];
            messages = elements["messages"] as List<SlackMessage>;
            has_more = (bool) elements["has_more"];
        }
    }
}
