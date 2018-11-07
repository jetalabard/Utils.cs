using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Serialization.Default;

namespace Utils.ReadWrite.Writer.Standard
{
    public class DefaultWriter<T> :AbstractWriter<T>, IWriter<T> where T : Serializable
    {
        private DefaultSerializer<T> _DefaultSerializer
        {
            get
            {
                return ConvertSerializer<DefaultSerializer<T>>();
            }
        }

        /// <summary>
        /// allows to write fields define in your BasicSerializer implementation 
        /// separate by 'string' define in BasicSerializer implementation 
        /// and with by 'string' define in BasicSerializer implementation as lines separator 
        /// </summary>
        /// <param name="serializer"></param>
        public DefaultWriter(IBasicSerializer<T> serializer)
        {
            _serializer = new DefaultSerializer<T>(serializer);
        }

        /// <summary>
        /// allows to write Public fields of T Object separate by space and with '\r\n' as lines separator 
        /// </summary>
        public DefaultWriter()
        {
            _serializer = new DefaultSerializer<T>(new DefaultBasicSerializer<T>());
        }

        public override void Write(T element, string path)
        {
            StandardWriter<T>.Write(_DefaultSerializer, element, path);
        }

        public override void Write<Y>(Y listElements, string path)
        {
            StandardWriter<T>.Write(_DefaultSerializer, listElements, path);
        }

        public override void Append<Y>(T element, string path)
        {
            StandardWriter<T>.Append<Y>(_DefaultSerializer, element, path);
        }

        public override void Append<Y>(ListSerializable<T> listElements, string path, string ElementsName = "")
        {
            string text = _DefaultSerializer.SerializeList<Y>(listElements);
            FileWriter.Append(text, path);
        }
    }
}
