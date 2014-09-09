using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.XamarinThings.Core
{
    public static class XCorePlatform
    {
        public static XCorePlatforms Platform { get; set; }
        public enum XCorePlatforms
        {
            iOS,
            Android
        }
    }


}
