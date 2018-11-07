using System.Runtime.Serialization;
using Utils.ReadWrite.Serialization;

namespace UnitTest.SerializeDeserialize
{
    [CollectionDataContract(Name="users", Namespace = "",IsReference =false)]
    [KnownType(typeof(ListSerializable<User>))]
    [KnownType(typeof(User))]
    [KnownType(typeof(Serializable))]
    public class UserList : ListSerializable<User>
    {
        public UserList() : base() { }
    }
}
