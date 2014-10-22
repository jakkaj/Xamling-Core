using Newtonsoft.Json;
using XamlingCore.Portable.Contract.Serialise;

namespace XamlingCore.Portable.Data.Serialise
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


            //there is a weird really annoying bug we cannot track that casues json to get an extra } on the end sometimes
            try
            {
                var resultCatch1 = JsonConvert.DeserializeObject<T>(entity);
                return resultCatch1;
            }
            catch
            {
                entity = entity.Substring(0, entity.Length - 1);
                var resultCatch2 = JsonConvert.DeserializeObject<T>(entity);
                return resultCatch2;
            }
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
