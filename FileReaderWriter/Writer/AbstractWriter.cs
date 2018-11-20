using System.Reflection;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.Reflection;

namespace Utils.FileReaderWriter
{
    public abstract class AbstractWriter<T> : IWriter<T> where T: Serializable
    {
        internal ISerializer<T> _serializer;

        internal X ConvertSerializer<X>() where X: ISerializer<T>
        {
            return (X)_serializer;
        }
        public abstract void Append<Y>(T element, string path) where Y : ListSerializable<T>;
        
        public abstract void Append<Y>(ListSerializable<T> listElements, string path, string ElementsName = "") where Y : ListSerializable<T>;

        public abstract void Write(T element, string path);

        public void Write<Y>(ListSerializable<T> listElements, string path) where Y : ListSerializable<T>
        {
            Write((Y)listElements, path);
        }
        public void setBinding(BindingFlags flags) 
        {
            AccessProperty.setBinding(flags);
        }

        public abstract void Write<Y>(Y listElements, string path) where Y : ListSerializable<T>;
    }
}
