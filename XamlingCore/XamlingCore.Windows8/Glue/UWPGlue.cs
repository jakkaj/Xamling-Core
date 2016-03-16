using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Glue;
using XamlingCore.Windows8.Glue.Modules;
using XamlingCore.XamarinThings.Glue;

namespace XamlingCore.Windows8.Glue
{
    public class UWPGlue : GlueBase
    {
        public override void Init()
        {
            base.Init();
            Builder.RegisterModule<DefaultXCoreModule>();
            Builder.RegisterModule<DefaultNativeUWPModule>();
            Builder.RegisterModule<XamarinGlue>();
         
        }
    }
}
