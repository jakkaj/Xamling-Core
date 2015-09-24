namespace XamlingCore.Portable.Contract.Serialise
{
    public interface IEntitySerialiser
    {
        T Deserialise<T>(string entity)
            where T : class;

        string Serialise<T>(T entity);
        T BinaryDeserialise<T>(byte[] entity) where T : class;
        byte[] BinarySerialise<T>(T entity);
    }
}