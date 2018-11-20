using System;

namespace Utils.FileReaderWriter.Serialization.Reflection
{
    internal static class CreateInstanceManager<T> where T : Serializable
    {
        internal static Y CreateInstance<Y>() where Y : ListSerializable<T>
        {
            return (Y)Activator.CreateInstance(typeof(Y));
        }

        internal static T CreateInstance(StringList parameters) 
        {
            T element;
            try
            {
                element = (T)Activator.CreateInstance(typeof(T), new object[] { parameters });
            }
            catch (MissingMethodException)
            {
                throw new MissingMethodException("Your class must implemented constructor with StringList as parameter");
            }
            
            return element;
        }
    }
}
