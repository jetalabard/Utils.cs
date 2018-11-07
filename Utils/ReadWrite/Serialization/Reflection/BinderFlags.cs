using System.Reflection;

namespace Utils.ReadWrite.Serialization.Reflection
{
    public abstract class BinderFlags
    {
        /// <summary>
        /// allows to change binding which allows to find property of T object
        /// by default bindingFlags = BindingFlags.Instance | BindingFlags.Static |BindingFlags.NonPublic |BindingFlags.Public;
        /// </summary>
        /// <param name="flags"></param>
        public abstract void setBinding(BindingFlags flags);
    }
}
