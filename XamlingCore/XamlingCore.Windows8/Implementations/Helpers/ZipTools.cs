using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Helpers;

namespace XamlingCore.Windows8.Implementations.Helpers
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

        public async Task<List<Tuple<string, byte[]>>> ExtractZip(byte[] zipData)
        {
            var tuples = new List<Tuple<string, byte[]>>();

            using (var ms = new MemoryStream(zipData))
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        var fn = entry.Name;
                        using (var stream = entry.Open())
                        {
                            var b = new byte[stream.Length];
                            stream.Read(b, 0, b.Length);
                            tuples.Add(new Tuple<string, byte[]>(fn, b));
                        }
                    }
                }
            }
            return tuples;
        }
    }
}
