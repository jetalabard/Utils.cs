using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Utils.FileReaderWriter.Serialization
{
    [DataContract(Namespace = "")]
    public abstract class Serializable
    {
        /// <summary>
        /// list of elements which represent properties of readable class
        /// </summary>
        private Dictionary<string, object> m_listOfElements;

        /// <summary>
        /// constructor which call child method Construct which allows to define properties of child from Stringlist
        /// </summary>
        /// <param name="listOfElements"></param>
        protected Serializable(Dictionary<string, object> listOfElements)
        {
            setElements(listOfElements);
            Construct();
        }


        /// <summary>
        /// build readable object with stringlist
        /// </summary>
        /// <returns></returns>
        public abstract void Construct();

        /// <summary>
        /// get list of elements which represent properties of readable class
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> getElements()
        {
            return m_listOfElements;
        }

        /// <summary>
        /// set list of elements which represent properties of readable class
        /// </summary>
        /// <param name="elements"></param>
        public void setElements(Dictionary<string, object> elements)
        {
            m_listOfElements = elements;
        }
    }
}
