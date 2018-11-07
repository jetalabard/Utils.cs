using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Serialization.Default;

namespace Utils.ReadWrite.Reader
{
    public class DefaultReader<T> : IReader<T> where T : Serializable
    {
        private readonly DefaultSerializer<T> _DefaultSerializer;

        public DefaultReader(IBasicSerializer<T> serializer)
        {
            _DefaultSerializer = new DefaultSerializer<T>(serializer);
        }
        public DefaultReader()
        {
            _DefaultSerializer = new DefaultSerializer<T>(new DefaultBasicSerializer<T>());
        }
        
        public ListSerializable<T> read<Y>(string filePath) where Y : ListSerializable<T>
        {
            string content = FileReader.Read(filePath);
            return _DefaultSerializer.DeserializeList<Y>(content);
        }

        Y IGenericReader<T>.read<Y>(string filePath)
        {
            return (Y)read<Y>(filePath);
        }
    }
}
