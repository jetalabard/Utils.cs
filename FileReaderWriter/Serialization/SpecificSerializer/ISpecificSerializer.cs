using System.Collections.Generic;

namespace Utils.FileReaderWriter.Serialization.SpecificSerializer
{
    public interface ISpecificSerializer<T> : ISerializer<T> where T : Serializable
    {
        T Deserialize(object textSerialized);

        Y DeserializeList<Y>(ICollection<object> textListSerialized) where Y : ListSerializable<T>;

        object[] Serialize(T objectToSerialize);

        ICollection<object[]> SerializeList<Y>(ListSerializable<T> listObjects) where Y : ListSerializable<T>;

        ICollection<object[]> SerializeList<Y>(Y listObjects) where Y : ListSerializable<T>;

    }

}