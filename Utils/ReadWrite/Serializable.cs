namespace Utils
{
    public class Serializable
    {
        /// <summary>
        /// list of elements which represent properties of readable class
        /// </summary>
        private StringList m_listOfElements;

        /// <summary>
        /// constructor which call child method Construct which allows to define properties of child from Stringlist
        /// </summary>
        /// <param name="listOfElements"></param>
        public Serializable(StringList listOfElements)
        {
            setElements(listOfElements);
            Construct();
        }

        /// <summary>
        /// build readable object with stringlist
        /// </summary>
        /// <returns></returns>
        public virtual void Construct()
        {
            throw new System.Exception("Error : the method Construct will be redefine in child class");
        }

        /// <summary>
        /// get list of elements which represent properties of readable class
        /// </summary>
        /// <returns></returns>
        public StringList getElements()
        {
            return m_listOfElements;
        }

        /// <summary>
        /// set list of elements which represent properties of readable class
        /// </summary>
        /// <param name="elements"></param>
        public void setElements(StringList elements)
        {
            m_listOfElements = elements;
        }


    }
}
