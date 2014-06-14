using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Serialise;

namespace XamlingCore.Portable.Serialise
{
    public class JsonEntitySerialiser : IEntitySerialiser
    {
        public T Deserialise<T>(string entity)
           where T : class
        {
            try
            {
                var des = new DataContractJsonSerializer(typeof(T));

                var ms = new MemoryStream(Encoding.UTF8.GetBytes(entity));

                T item = des.ReadObject(ms) as T;
                return item;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Des problem: " + ex.Message);
            }

            return null;
        }


        public string Serialise<T>(T entity)
        {
            var des = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();
            des.WriteObject(ms, entity);

            ms.Seek(0, SeekOrigin.Begin);

            var b = new byte[ms.Length];

            ms.Read(b, 0, b.Length);

            return Encoding.UTF8.GetString(b, 0, b.Length);
        }
    }
}
