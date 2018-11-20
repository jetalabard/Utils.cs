using System.Collections.Generic;
using Utils.FileReaderWriter.Serialization;

namespace Util.FileReaderWriters.Serialization
{
    public class StringSerializable : Serializable
    {
        public string SimpleString
        {
            get;
            private set;
        }


        public StringSerializable(string value): base(new Dictionary<string, object> { { "value", value } })
        {
        }
        
        public override void Construct()
        {
            Dictionary<string, object> elements = getElements();
            SimpleString = elements["value"] as string;
        }
    }
}
