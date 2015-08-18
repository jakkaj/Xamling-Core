namespace XamlingCore.Portable.Contract.Serialise
{
    public interface IEntitySerialiser
    {
        T Deserialise<T>(string entity)
            where T : class;

        string Serialise<T>(T entity);
    }
}