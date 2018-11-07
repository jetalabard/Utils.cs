using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Utils.ReadWrite.Serialization.Reflection;
using Utils.ReadWrite.Serialization.StandardSerializer;

namespace Utils.ReadWrite.Serialization.Default
{
    public class DefaultSerializer<T> : BinderFlags, IStandardSerializer<T> where T : Serializable
    {
        private readonly IBasicSerializer<T> _serializer;

        public DefaultSerializer(IBasicSerializer<T> basicSerializer)
        {
            _serializer = basicSerializer;
        }


        public DefaultSerializer()
        {
            _serializer = new DefaultBasicSerializer<T>();
        }

        public override void setBinding(BindingFlags flags)
        {
            AccessProperty.setBinding(flags);
        }

        public Y DeserializeList<Y>(string textListSerialized)
            where Y : ListSerializable<T>
        {
            StringList lines = new StringList(textListSerialized.Split(_serializer.SeparatorsLine()));
            Y elements = (Y)Activator.CreateInstance(typeof(Y));
            foreach (string text in lines.Where(l => !string.IsNullOrEmpty(l)))
            {
                elements.Add(Deserialize(text));
            }
            return elements;
        }


        public T Deserialize(string textSerialized)
        {
            return _serializer.StringToObject(textSerialized);
        }

        public string Serialize(T objectToSerialize)
        {
            return _serializer.ToString(objectToSerialize);
        }
        
        public string SerializeList<Y>(Y listObjects) where Y : ListSerializable<T>
        {
            StringBuilder sb = new StringBuilder();
            foreach (T obj in listObjects)
            {
                sb.Append(Serialize(obj));
                sb.Append(_serializer.SeparatorsLine());
            }
            return sb.ToString();
        }

        public string SerializeList<Y>(ListSerializable<T> listObjects) where Y : ListSerializable<T>
        {
            return SerializeList((Y)listObjects);
        }

    }
}
