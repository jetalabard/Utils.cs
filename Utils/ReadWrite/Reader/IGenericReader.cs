using Utils.ReadWrite.Serialization;

namespace Utils.ReadWrite.Reader
{
    public interface IGenericReader<T> where T: Serializable
    {
        Y read<Y>(string filePath) where Y : ListSerializable<T>;
    }
}
