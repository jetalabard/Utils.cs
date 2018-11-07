using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Utils.ReadWrite.Serialization
{
    [DataContract(Namespace = "")]
    public abstract class Serializable
    {
        /// <summary>
        /// list of elements which represent properties of readable class
        /// </summary>
        private Dictionary<string, string> m_listOfElements;

        /// <summary>
        /// constructor which call child method Construct which allows to define properties of child from Stringlist
        /// </summary>
        /// <param name="listOfElements"></param>
        protected Serializable(Dictionary<string, string> listOfElements)
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
        public Dictionary<string, string> getElements()
        {
            return m_listOfElements;
        }

        /// <summary>
        /// set list of elements which represent properties of readable class
        /// </summary>
        /// <param name="elements"></param>
        public void setElements(Dictionary<string, string> elements)
        {
            m_listOfElements = elements;
        }
    }
}
