using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Foundation;
using UIKit;
using XamlingCore.Portable.Glue.Glue;

namespace XamlingCore.iOS.Unified.Glue
{
    public static class iOSXCoreAutoRegistration
    {
        public static void RegisterAssembly(ContainerBuilder builder, Type typeInAssembly, bool propertiesAutowired = false)
        {
            builder.RegisterModule(new XAutofacModule(typeInAssembly.Assembly, "View", true, false));
            builder.RegisterModule(new XAutofacModule(typeInAssembly.Assembly, "ViewModel", true, false, propertiesAutowired));
        }
    }
}