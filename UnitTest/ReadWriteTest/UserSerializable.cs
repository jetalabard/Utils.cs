using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.Reader;

namespace UnitTest
{
    [DataContract]
    public class UserSerializable : Serializable
    {
        /// <summary>
        /// name of user
        /// </summary>
        [DataMember]
        public string Name
        {
            get;private set;
        }

        /// <summary>
        /// firstname of user
        /// </summary>
        [DataMember]
        public string Firstname
        {
            get; private set;
        }

        /// <summary>
        /// constructor which define all properties
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Firstname"></param>
        public UserSerializable(string Name, string Firstname):base(new StringList() { Name, Firstname })
        {
            //call construct method in parent class
        }

        public UserSerializable(StringList elements) : base(elements)
        {
            //call construct method in parent class
        }

        /// <summary>
        /// construct user from list of elements
        /// </summary>
        public override void Construct()
        {
            StringList elements = getElements();
            if (elements.Count == 2)
            {
                Name = elements.ElementAt(0);
                Firstname = elements.ElementAt(1);
            }
            else
            {
                throw new ArgumentException("Error to build User readable because list of elements doesn't contains 2 elements");
            }
        }

    }
}
