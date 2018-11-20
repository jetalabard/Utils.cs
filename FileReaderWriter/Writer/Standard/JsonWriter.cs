using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using Utils.FileReaderWriter.Serialization;
using Utils.FileReaderWriter.Serialization.StandardSerializer;

namespace Utils.FileReaderWriter.Standard
{
    public class JsonWriter<T> : AbstractWriter<T>, IWriter<T> where T: Serializable
    {
        private JsonSerializer<T> _JsonSerializer
        {
            get
            {
                return ConvertSerializer<JsonSerializer<T>>();
            }
        }

        public JsonWriter()
        {
            _serializer = new JsonSerializer<T>();
            setBinding(BindingFlags.Public | BindingFlags.Instance); // by default
        }

        public override void Append<Y>(T element, string path)
        {
            StandardWriter<T>.Append<Y>(_JsonSerializer, element, path);
        }

        public override void Append<Y>(ListSerializable<T> listElements, string path, string ElementsName = "")
        {
            if (File.Exists(path))
            {
                string text = _JsonSerializer.SerializeList<Y>(listElements);
                JArray root = (JArray)JsonConvert.DeserializeObject(File.ReadAllText(path));
                JArray parentList = JArray.Parse(text);
                foreach (var item in parentList.Children())
                {
                    root.Add(item);
                }
                FileWriter.Write(root.ToString(), path);
            }
            else
            {
                throw new FileNotFoundException(path + "not found");
            }
        }

        public override void Write(T element, string path)
        {
            StandardWriter<T>.Write(_JsonSerializer, element, path);
        }

        public override void Write<Y>(Y listElements, string path)
        {
            StandardWriter<T>.Write<Y>(_JsonSerializer, listElements, path);
        }

        
    }
}
