using System.Collections.Generic;
using System.Linq;

namespace XamlingCore.Portable.Model.Localisation
{
    public class XLocalisedResources
    {
        public List<XResource> Resources { get; set; }
        public string XResourceId { get; set; }

        public XLocalisedResources()
        {
            Resources = new List<XResource>();
        }

        public string GetResource(string name, string culture)
        {
            var res = Resources.FirstOrDefault(_ => _.CultureName == culture);

            if (res == null)
            {
                return null;
            }

            var item = res.Values.FirstOrDefault(_ => _.Name == name);

            return item == null ? null : item.Value;
        }

        public void AddResource(string name, string value, string culture)
        {
            var res = Resources.FirstOrDefault(_ => _.CultureName == culture);

            if (res == null)
            {
                res = new XResource { CultureName = culture };
                Resources.Add(res);
            }

            var item = res.Values.FirstOrDefault(_ => _.Name == name);

            if (item == null)
            {
                item = new XValue() { Name = name };
                res.Values.Add(item);
            }

            item.Value = value;
        }


    }

    public class XResource
    {
        public XResource()
        {
            Values = new List<XValue>();
        }
        public string CultureName { get; set; }
        public List<XValue> Values { get; set; }
    }


    public class XValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
