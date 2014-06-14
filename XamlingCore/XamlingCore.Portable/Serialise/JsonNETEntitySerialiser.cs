using Newtonsoft.Json;
using XamlingCore.Portable.Contract.Serialise;

namespace XamlingCore.Portable.Serialise
{
    public class JsonNETEntitySerialiser : IEntitySerialiser
    {
        public JsonSerializerSettings Settings { get; set; }

        public T Deserialise<T>(string entity) where T : class
        {
            if (Settings != null)
            {
                return JsonConvert.DeserializeObject<T>(entity, Settings);
            }

            var result = JsonConvert.DeserializeObject<T>(entity);

            return result;
            
            
        }

        public string Serialise<T>(T entity)
        {
            if (Settings != null)
            {
                return JsonConvert.SerializeObject(entity, Formatting.None, Settings);
            }
            return JsonConvert.SerializeObject(entity, Formatting.None);
        }
    }
}
