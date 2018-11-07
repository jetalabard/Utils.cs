using Utils.ReadWrite.Serialization;

namespace Utils.ReadWrite.Reader
{
    public interface IReader<T> : IGenericReader<T> where T : Serializable
    {

        /// <summary>
        /// allows to read a file
        /// </summary>
        /// <param name="filePath">path of the file</param>
        /// <returns>list of each object contains in file</returns>
        new  ListSerializable<T> read<Y>(string filePath) where Y : ListSerializable<T>;
    }
}
