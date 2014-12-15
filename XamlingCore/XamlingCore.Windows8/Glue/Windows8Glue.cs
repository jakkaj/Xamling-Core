using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Glue;
using XamlingCore.Windows8.Glue.Modules;

namespace XamlingCore.Windows8.Glue
{
    public class Windows8Glue : GlueBase
    {
        public override void Init()
        {
            base.Init();
            Builder.RegisterModule<DefaultXCoreModule>();
            Builder.RegisterModule<DefaultWindows8Module>();

         
        }
    }
}
