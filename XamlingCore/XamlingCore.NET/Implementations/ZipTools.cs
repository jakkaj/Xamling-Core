using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Helpers;

namespace XamlingCore.NET.Implementations
{
    public class Zip : IZip
    {
        public byte[] CreateZip(params Tuple<string, byte[]>[] files)
        {
            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create))
                {
                    foreach (var item in files)
                    {
                        var entry = archive.CreateEntry(item.Item1, CompressionLevel.Optimal);
                        using (var msWriter = entry.Open())
                        {
                            msWriter.Write(item.Item2, 0, item.Item2.Length);
                        }
                    }
                }
             
                return ms.ToArray();
            }
        }
    }
}
