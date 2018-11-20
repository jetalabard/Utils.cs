namespace Utils.FileReaderWriter.Serialization.StandardSerializer
{
    public interface IStandardSerializer<T> : ISerializer<T> where T : Serializable
    {
        /// <summary>
        /// serialize generic object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        string Serialize(T objectToSerialize);

        /// <summary>
        /// serialize generic object list
        /// </summary>
        /// <typeparam name="T">type of liste</typeparam>
        /// <typeparam name="Y">type of object in list</typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        string SerializeList<Y>(Y listObjects) where Y : ListSerializable<T>;
        string SerializeList<Y>(ListSerializable<T> listObjects) where Y : ListSerializable<T>;


        /// <summary>
        /// deserialize generic object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        T Deserialize(string textSerialized);

        /// <summary>
        /// deserialize generic object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        Y DeserializeList<Y>(string textListSerialized) where Y : ListSerializable<T>;

    }
}
