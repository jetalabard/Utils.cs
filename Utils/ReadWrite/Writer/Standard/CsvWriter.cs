using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Utils.ReadWrite.Reader;
using Utils.ReadWrite.Serialization;
using Utils.ReadWrite.Serialization.StandardSerializer;

namespace Utils.ReadWrite.Writer.Standard
{
    public class CsvWriter<T> : AbstractWriter<T>, IWriter<T> where T: Serializable
    {

        private CsvSerializer<T> _CsvSerializer
        {
           get
            {
                return ConvertSerializer<CsvSerializer<T>>();
            }
        }
    
        public CsvWriter(char separator, StringList headers = null)
        {
            _serializer = new CsvSerializer<T>(separator, headers);
        }

        public override void Append<Y>(T element, string path)
        {
            StandardWriter<T>.Append<Y>(_CsvSerializer, element, path);
        }
        

        public override void Append<Y>(ListSerializable<T> listElements, string path, string ElementsName = "")
        {
            string text = _CsvSerializer.SerializeList<Y>(listElements);
            if (_CsvSerializer.HasHeader)
            {
                Type type = typeof(T);
                var bindingFlags = BindingFlags.Instance |
                                  BindingFlags.NonPublic |
                                  BindingFlags.Public;
                FieldInfo[] listValues = type.GetFields(bindingFlags);
                if (listValues.Length > 0)
                {

                    StringList listValuesName = new StringList(listValues.Select(value => value.Name).ToList());
                    string header = listValuesName.Join(_CsvSerializer.Separator);
                    StringList linesInfile = new CsvReader<T>(_CsvSerializer.Separator).readLine(path, true);
                    if (linesInfile.First() != header)
                    {
                        //add header and rewrite file
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(header);
                        foreach (string line in linesInfile)
                        {
                            sb.AppendLine(line);
                        }
                        FileWriter.Write(sb.ToString(), path);
                    }

                    if (text.Contains(header))
                    {
                        //remove header if exist in text to append
                        text = text.Remove(0, header.Length + 2); //2 for \r\n
                    }
                }
            }
            FileWriter.Append(text, path);
        }

        public override void Write(T element, string path)
        {
            StandardWriter<T>.Write(_CsvSerializer, element, path);
        }

        public override void Write<Y>(Y listElements, string path)
        {
            StandardWriter<T>.Write<Y>(_CsvSerializer, listElements, path);
        }
    }
}
