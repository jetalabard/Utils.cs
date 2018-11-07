using System;
using Utils.ReadWrite.Serialization;

namespace Utils.ReadWrite.Writer
{
    public abstract class AbstractWriter<T> : IWriter<T> where T: Serializable
    {
        internal ISerializer<T> _serializer;

        internal X ConvertSerializer<X>() where X: ISerializer<T>
        {
            if(_serializer is X)
            {
                return (X)_serializer;
            }
            throw new InvalidCastException("Serializer is not an instance of " + typeof(X).Name);
        }
        public abstract void Append<Y>(T element, string path) where Y : ListSerializable<T>;
        
        public abstract void Append<Y>(ListSerializable<T> listElements, string path, string ElementsName = "") where Y : ListSerializable<T>;

        public abstract void Write(T element, string path);

        public void Write<Y>(ListSerializable<T> listElements, string path) where Y : ListSerializable<T>
        {
            Write((Y)listElements, path);
        }

        public abstract void Write<Y>(Y listElements, string path) where Y : ListSerializable<T>;
    }
}
