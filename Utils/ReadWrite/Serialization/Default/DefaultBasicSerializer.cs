using System;
using System.Reflection;
using System.Text;
using Utils.ReadWrite.Serialization.Reflection;

namespace Utils.ReadWrite.Serialization.Default
{
    public class DefaultBasicSerializer<T> :IBasicSerializer<T> where T : Serializable
    {
        public string SeparatorsColumn()
        {
            return " ";
        }

        public string SeparatorsLine()
        {
            return "\r\n";
        }

     

        public T StringToObject(string objectSerialize)
        {
          StringList list = new StringList( objectSerialize.Split(SeparatorsColumn()));
            return CreateInstanceManager<T>.CreateInstance(list);
        }

        public string ToString(T item)
        {
            Type type = typeof(T);
            StringBuilder csvdata = new StringBuilder();
            FieldInfo[] fields = AccessProperty.GetFields(type);
            string[] fieldsString = AccessProperty.FieldsToString(fields, item);
            for(int i = 0; i< fieldsString.Length;i++)
            {
                csvdata.Append(fieldsString[i]);
                if(i < fieldsString.Length-1)
                {
                    csvdata.Append(SeparatorsColumn());
                }
            }
           
            return csvdata.ToString();
        }
    }
}
