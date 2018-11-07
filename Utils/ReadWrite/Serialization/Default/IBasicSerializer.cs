namespace Utils.ReadWrite.Serialization.Default
{
    public interface IBasicSerializer<T>  where T : Serializable
    {
        string ToString(T item);

        T StringToObject(string objectSerialize);

        string SeparatorsLine();

        string SeparatorsColumn();
    }
}
