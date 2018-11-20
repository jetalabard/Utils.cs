using System.Collections.Generic;
using System.Runtime.Serialization;
using Utils;
using Utils.FileReaderWriter.Serialization;

namespace UnitTest.SerializeDeserialize
{

    [DataContract(Name = "user", Namespace = "")]
    [KnownType(typeof(string))]
    public class User : Serializable
    {
        /// <summary>
        /// name of user
        /// </summary>
        [DataMember(Order = 2,Name = "Name")]
        public string Name;

        /// <summary>
        /// firstname of user
        /// </summary>
        [DataMember(Order = 1, Name = "Firstname")]
        public string Firstname;

        /// <summary>
        /// constructor which define all properties
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Firstname"></param>
        public User(string Name, string Firstname)
            :base(new Dictionary<string, object>() { { "Name",Name }, { "Firstname", Firstname } })
        {
            //call construct method in parent class
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="elements"></param>
        public User(StringList elements) :this(elements[0], elements[1])
        {
        }

        /// <summary>
        /// construct user from list of elements
        /// </summary>
        public override void Construct()
        {
            Dictionary<string, object> elements = getElements();
            Name = elements["Name"] as string;
            Firstname = elements["Firstname"] as string;
        }

        /// <summary>
        /// useful for Default serializer
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + " " + Firstname;
        }

    }
}
