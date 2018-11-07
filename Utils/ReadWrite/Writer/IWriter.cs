using Utils.ReadWrite.Serialization;

namespace Utils.ReadWrite.Writer
{
    public interface IWriter<T> where T : Serializable
    {
        void Write<Y>(ListSerializable<T> listElements, string path) where Y : ListSerializable<T>;

        void Write(T element, string path);

        void Append<Y>(ListSerializable<T> listElements, string path, string ElementsName = "") where Y : ListSerializable<T>;

        void Append<Y>(T element, string path) where Y : ListSerializable<T>;
    }
}
