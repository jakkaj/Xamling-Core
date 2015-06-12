namespace XamlingCore.Portable.Contract.Helpers
{
    public interface IHashHelper
    {
        string Hash(byte[] data);
        string HMAC256(string input, string key);
    }
}