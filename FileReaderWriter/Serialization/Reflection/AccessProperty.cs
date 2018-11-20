using System;
using System.Reflection;
using System.Linq;

namespace Utils.FileReaderWriter.Serialization.Reflection
{
    internal static class AccessProperty
    {
        private static BindingFlags _Flags = BindingFlags.Instance | BindingFlags.Public;

        internal static void setBinding(BindingFlags flags)
        {
            _Flags = flags;
        }


        /// <summary>
        /// get fields of object
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="fields"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        internal static string[] FieldsToString(FieldInfo[] fields, object o)
        {
            string[] linie = new string[fields.Length];

            for(int i=0; i< fields.Length;i++)
            {
                var x = fields[i].GetValue(o);

                if (x != null)
                    linie[i] = x.ToString();
            }

            return linie;
        }


        /// <summary>
        /// get fields of object
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="fields"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        internal static FieldInfo[] GetFields(Type type)
        {
            FieldInfo[] fields = type.GetFields(_Flags);
            if(fields == null)
            {
                throw new InvalidOperationException("Unable to access fields of type " + type.Name);
            }

            if (fields.Length == 0)
            {
                throw new InvalidOperationException("No fields of type " + type.Name);
            }
            return fields;
        }

        /// <summary>
        /// get fields of object
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="fields"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        internal static FieldInfo GetField(Type type,string fieldName)
        {
            return type.GetField(fieldName, _Flags);
        }


        internal static void CkeckIfPropertiesToSerializeExist(FieldInfo[] fields, StringList propertiesToSerialize, Type type)
        {
            if (propertiesToSerialize.Count > fields.Length)
            {
                throw new ArgumentException("There are too much properties to serialize (" + propertiesToSerialize.Join(' ') +" ) in class " + type.Name);
            }

            foreach (string property in propertiesToSerialize)
            {
                FieldInfo prop = GetField(type,property);
                if (prop == null)
                {
                    throw new FieldAccessException("Unable to access of property " + property + " of type  " + type.Name);
                }
            }
        }
    }
}
