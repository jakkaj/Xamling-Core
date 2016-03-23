using System;
using System.Reflection;
using Autofac;
using XamlingCore.Portable.Glue.Glue;

namespace XamlingCore.UWP.Glue
{
    public static class XUWPCoreAutoRegistration
    {
        public static void RegisterAssembly(ContainerBuilder builder, Type typeInAssembly, bool propertiesAutowired = false)
        {
            builder.RegisterAssemblyTypes(typeInAssembly.GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("View"))
                .AsSelf()
                .AsImplementedInterfaces();


            //builder.RegisterModule(new XAutofacModule(typeInAssembly.GetTypeInfo().Assembly, "View", true, false));
            builder.RegisterModule(new XAutofacModule(typeInAssembly.GetTypeInfo().Assembly, "ViewModel", true, false, propertiesAutowired));
        }
    }
}
