using Newtonsoft.Json.Linq;
using System;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.StandardSerializer;

namespace Utils.FileReaderWriter.Reader
{
    public class JsonReader<T> : IReader<T> where T : Serializable
    {
        public ListSerializable<T> read<Y>(string filePath)
            where Y : ListSerializable<T>
        {
            JsonSerializer<T> serializer = new JsonSerializer<T>();
            string jsonContent = FileReader.Read(filePath);
            Y returnList = (Y)Activator.CreateInstance(typeof(Y));
            try
            {
                var token = JToken.Parse(jsonContent);
                if (token is JArray)
                {
                    returnList = serializer.DeserializeList<Y>(jsonContent);
                }
                else if (token is JObject)
                {
                    returnList.Add(serializer.Deserialize(jsonContent));
                }
            }
            catch
            {
                throw new ArgumentException("File not contains this type of serializable object");
            }
            return returnList;
        }

        Y IGenericReader<T>.read<Y>(string filePath)
        {
            return (Y)read<Y>(filePath);
        }

    }
}
