using System;
using Autofac;
using XamlingCore.Portable.Glue.Glue;

namespace XamlingCore.Platform.Shared.Glue
{
    public static class XCoreAutoRegistration
    {
        public static void RegisterAssembly(ContainerBuilder builder, Type typeInAssembly, bool propertiesAutowired = false)
        {
            builder.RegisterModule(new XAutofacModule(typeInAssembly.Assembly, "View", true, false));
            builder.RegisterModule(new XAutofacModule(typeInAssembly.Assembly, "ViewModel", true, false, propertiesAutowired));
        }
    }
}
