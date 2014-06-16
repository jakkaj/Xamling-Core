using System.Xml.Linq;
using System.Xml.Serialization;

namespace XamlingCore.Portable.Data.Serialise
{
    public class XMLSerialisationHelper
    {

        public static TEntity DeSerialiseEntity<TEntity>(string serialisedEntity)
        {
            var document = XDocument.Parse(serialisedEntity);
            var serializer = new XmlSerializer(typeof(TEntity));
            var result = (TEntity)serializer.Deserialize(document.CreateReader());
            return result;
        }
    }
}
