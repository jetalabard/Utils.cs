using Utils.FileReaderWriter.Serialization;

namespace Utils.FileReaderWriter.Reader
{
    public interface IGenericReader<T> where T: Serializable
    {
        Y read<Y>(string filePath) where Y : ListSerializable<T>;
    }
}
