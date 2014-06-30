using System;
using XamlingCore.Portable.Glue.Glue;

namespace XamlingCore.Platform.Shared.Glue
{
    public static class XAutofacAutoRegisterExtensions
    {
        public static XAutofacModule AutofacRegister(this Type type, string filter, bool asSelf = false, bool singleInstance = true)
        {
            return new XAutofacModule(type.Assembly, filter, asSelf, singleInstance);
        }
    }
}