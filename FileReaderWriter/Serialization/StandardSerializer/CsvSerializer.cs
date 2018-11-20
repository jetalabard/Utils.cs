using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Utils.FileReaderWriter.Serialization.Reflection;

namespace Utils.FileReaderWriter.Serialization.StandardSerializer
{
    public class CsvSerializer<T> : BinderFlags,IStandardSerializer<T> where T:Serializable
    {
        /// <summary>
        /// csv separator
        /// </summary>
        private readonly char _separator;
        public char Separator {
            get
            {
                return _separator;
            }
        }

        /// <summary>
        /// headers to serialize
        /// </summary>
        private readonly StringList _headers;
        /// <summary>
        /// allows to knows if file must be managed with header
        /// </summary>
        public bool HasHeader
        {
            get
            {
                return _headers != null;
            }
        }


        /// <summary>
        /// constructor
        /// if headers is null then there is no header line
        /// </summary>
        /// <param name="separator"></param>
        public CsvSerializer(char separator, StringList headers = null)
        {
            _separator = separator;
            _headers = headers;
        }

        /// <summary>
        /// Deserialize Csv object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        public T Deserialize(string textSerialized)
        {
            StringList listElements = new StringList(textSerialized.Split(_separator));
            T element = CreateInstanceManager<T>.CreateInstance(listElements);
            element.Construct();
            return element;
        }
        /// <summary>
        /// Deserialize Csv object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        public Y DeserializeList<Y>(string textListSerialized) where Y : ListSerializable<T>
        {
            StringList lines = new StringList(textListSerialized.Split("\r\n"));
            if (_headers != null && _headers.Join(_separator) == lines[0])
            {
                //there is a header line
                lines.Remove(lines[0]);
            }
            Y elements = CreateInstanceManager<T>.CreateInstance<Y>();
            foreach (string text in lines.Where(l => !string.IsNullOrEmpty(l)))
            {
                elements.Add(Deserialize(text));
            }
            return elements;
        }

        /// <summary>
        /// Serialize Csv object
        /// doesn't show header
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public string Serialize(T objectToSerialize)
        {
            Type type = typeof(T);
            StringBuilder csvdata = new StringBuilder();
            FieldInfo[] fields = AccessProperty.GetFields(type);
            if (_headers != null && _headers.Count > 0 && fields != null && fields.Length != 0)
            {
                AccessProperty.CkeckIfPropertiesToSerializeExist(fields, _headers, type);
                string header = _headers.Join(_separator);
                csvdata.AppendLine(header);
                fields = fields.Where(field => _headers.Contains(field.Name)).ToArray();
            }
            if(fields.Length > 0) {
                csvdata.AppendLine(ToCsvFields(fields, objectToSerialize, Convert.ToString(_separator)));
            }
            return csvdata.ToString();
        }

        /// <summary>
        /// Serialize Csv object list
        /// </summary>
        /// <typeparam name="T"> type of serializable object not type of list</typeparam>
        /// <param name="listObjects"></param>
        /// <returns></returns>
        public string SerializeList<Y>(Y listObjects) where Y : ListSerializable<T>
        {
            StringBuilder csvdata = new StringBuilder();
            Type type = typeof(T);
            FieldInfo[] listValues = AccessProperty.GetFields(type);
            if (_headers != null && _headers.Count > 0)
            {
                listValues = listValues.Where(field => _headers.Contains(field.Name)).ToArray();
                string header = _headers.Join(_separator);
                csvdata.AppendLine(header);
            }

            foreach (var obj in listObjects)
                csvdata.AppendLine(ToCsvFields(listValues, obj, Convert.ToString(_separator)));

            return csvdata.ToString();
        }

      

        public string SerializeList<Y>(ListSerializable<T> listObjects) where Y : ListSerializable<T>
        {
            return SerializeList((Y)listObjects);
        }

        private string ToCsvFields(FieldInfo[] fields, object o, string separator = ";")
        {
            StringBuilder sb = new StringBuilder();
            string[] values = AccessProperty.FieldsToString(fields, o);
            foreach (string value in values)
            {
                if(sb.Length > 0)
                {
                    sb.Append(separator);
                }
                sb.Append(value);
            }
            return sb.ToString();
        }

        public override void setBinding(BindingFlags flags)
        {
            AccessProperty.setBinding(flags);
        }
    }
}
