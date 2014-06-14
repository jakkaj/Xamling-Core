using System.IO;

namespace XamlingCore.Portable.Contract.Infrastructure.Resource
{
    public interface IApplicationResource
    {
        Stream GetResource(string name);
    }
}