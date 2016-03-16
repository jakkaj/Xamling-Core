using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Glue.Glue;
using XamlingCore.Windows8.View;

namespace XamlingCore.Windows8.Shared.Glue
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
