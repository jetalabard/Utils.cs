using Utils;
using Utils.FileReaderWriter.Serialization.Default;

namespace UnitTest.SerializeDeserialize.Serializer
{
    public class UserBasicSerializer : IBasicSerializer<User>
    {
        public User StringToObject(string objectSerialize)
        {
            string[] properties = objectSerialize.Split(SeparatorsColumn());
            return new User(properties[0],properties[1]);
        }

        public string ToString(User item)
        {
            return item.ToString();
        }

        public string SeparatorsColumn()
        {
            return " ";
        }

        public string SeparatorsLine()
        {
            return "\r\n";
        }
    }
}
