using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Glue.Glue;

namespace XamlingCore.Windows8.Shared.Glue
{
    public static class XAutofacAutoRegisterExtensions
    {
        public static XAutofacModule AutofacRegister(this Type type, string filter, bool asSelf = false, bool singleInstance = true)
        {
            return new XAutofacModule(type.GetTypeInfo().Assembly, filter, asSelf, singleInstance);
        }
    }
}
