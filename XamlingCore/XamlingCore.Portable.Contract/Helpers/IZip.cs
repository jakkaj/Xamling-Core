using System;

namespace XamlingCore.Portable.Contract.Helpers
{
    public interface IZip
    {
        byte[] CreateZip(params Tuple<string, byte[]>[] files);
    }
}