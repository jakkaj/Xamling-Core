using System;
using System.Reflection;
using XamlingCore.Portable.Glue.Glue;

namespace XamlingCore.UWP.Implementations
{
    public static class XAutofacAutoRegisterExtensions
    {
        public static XAutofacModule AutofacRegister(this Type type, string filter, bool asSelf = false, bool singleInstance = true)
        {
            return new XAutofacModule(type.GetTypeInfo().Assembly, filter, asSelf, singleInstance);
        }
    }
}
