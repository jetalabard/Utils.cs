using System.Collections.Generic;

namespace Utils.ReadWrite.Serialization
{
    public class StringSerializable : Serializable
    {
        public string SimpleString
        {
            get;
            private set;
        }


        public StringSerializable(string value): base(new Dictionary<string, string> { { "value", value } })
        {
        }
        
        public override void Construct()
        {
            Dictionary<string, string> elements = getElements();
            SimpleString = elements["value"];
        }
    }
}
