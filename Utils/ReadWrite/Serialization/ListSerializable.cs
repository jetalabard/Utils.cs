using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Utils.ReadWrite.Serialization
{
    [CollectionDataContract(Namespace ="")]
    [KnownType(typeof(Serializable))]
    public abstract class ListSerializable<T> : Collection<T> where T: Serializable
    {
        protected ListSerializable() : base()
        {

        }

        public void AddRange(ListSerializable<T> collection)
        {
            InsertRange(Count, collection);
        }
        /// <summary>
        /// insert a stringlist at specific position
        /// </summary>
        /// <param name="index"></param>
        /// <param name="collection"></param>
        public void InsertRange(int index, ListSerializable<T> collection)
        {
            if (collection != null)
            {
                using (IEnumerator<T> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Insert(index++, en.Current);
                    }
                }
            }
        }
    }
}
