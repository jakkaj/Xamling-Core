using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Helpers
{
    public interface IZip
    {
        byte[] CreateZip(params Tuple<string, byte[]>[] files);
        Task<List<Tuple<string, byte[]>>> ExtractZip(byte[] zipData);
    }
}