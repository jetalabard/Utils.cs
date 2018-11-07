using System;
using Utils.ReadWrite.Reader;
using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Serialization.Default;
using Utils.ReadWrite.Serialization.StandardSerializer;

namespace Utils.ReadWrite.Writer.Standard
{
    internal static class StandardWriter<T> where T: Serializable
    {

        internal static void Append<Y>(IStandardSerializer<T> serializer,T element, string path) where Y: ListSerializable<T>
        {
            string content = FileReader.Read(path);
            Y list = (Y)Activator.CreateInstance(typeof(Y));
            try
            {
                //check if it is a list
                list = serializer.DeserializeList<Y>(content);
                if (list.Count == 0)
                {
                    //maybe it's an object
                    list.Add(serializer.Deserialize(content));
                }
            }
            catch
            {
                //else is just an object
                list.Add(serializer.Deserialize(content));
            }

            list.Add(element);
            string text = serializer.SerializeList(list);
            FileWriter.Write(text, path);
        }


        internal static void Write(IStandardSerializer<T> serializer, T element, string path) 
        {
            string text = serializer.Serialize(element);
            FileWriter.Write(text, path);
        }

        internal static void Write<Y>(IStandardSerializer<T> serializer, Y listElements, string path) where Y : ListSerializable<T>
        {
            string text = serializer.SerializeList(listElements);
            FileWriter.Write(text, path);
        }
    }
}
