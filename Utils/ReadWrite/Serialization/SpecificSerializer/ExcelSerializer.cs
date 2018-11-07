using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utils.ReadWrite.Serialization.Reflection;

namespace Utils.ReadWrite.Serialization.SpecificSerializer
{
    public class ExcelSerializer<T>: BinderFlags, ISpecificSerializer<T>  where T : Serializable
    {
        internal readonly StringList _Properties;
        public ExcelSerializer(StringList propertiesToSerialize = null)
        {
            _Properties = propertiesToSerialize;
        }

        public T Deserialize(object textSerialized)
        {
            StringList list = textSerialized as StringList;
            T element = CreateInstanceManager<T>.CreateInstance(list);
            element.Construct();
            return element;
        }

        public Y DeserializeList<Y>(ICollection<object> textListSerialized) where Y : ListSerializable<T>
        {
            Y elements = CreateInstanceManager<T>.CreateInstance<Y>();
            foreach (StringList text in textListSerialized)
            {
                elements.Add(Deserialize(text));
            }
            return elements;
        }
        

        public object[] Serialize(T objectToSerialize)
        {
            Type type = typeof(T);
            FieldInfo[] fields = AccessProperty.GetFields(type);
            if (_Properties != null && _Properties.Count > 0)
            {
                AccessProperty.CkeckIfPropertiesToSerializeExist(fields, _Properties, type);
                fields = fields.Where(field => _Properties.Contains(field.Name)).ToArray();
            }

            return AccessProperty.FieldsToString(fields, objectToSerialize);
        }

        public ICollection<object[]> SerializeList<Y>(ListSerializable<T> listObjects) where Y : ListSerializable<T>
        {
            return SerializeList((Y)listObjects);
        }

        public ICollection<object[]> SerializeList<Y>(Y listObjects) where Y : ListSerializable<T>
        {
            Type type = typeof(T);
            ICollection<object[]> listArrayOfLine = new List<object[]>();
            FieldInfo[] fields = AccessProperty.GetFields(type);
            if (_Properties != null && _Properties.Count > 0)
            {
                AccessProperty.CkeckIfPropertiesToSerializeExist(fields, _Properties, type);
                fields = fields.Where(field => _Properties.Contains(field.Name)).ToArray();
            }

            foreach (var obj in listObjects)
                listArrayOfLine.Add(AccessProperty.FieldsToString(fields, obj));

            return listArrayOfLine;
        }

        public override void setBinding(BindingFlags flags)
        {
            AccessProperty.setBinding(flags);
        }
    }
}
