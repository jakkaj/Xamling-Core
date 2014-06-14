using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XamlingCore.Portable.Serialise
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
