using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Glue;
using XamlingCore.WP8.Glue.Modules;
using XamlingCore.XamarinThings.Glue;

namespace XamlingCore.WP8.Glue
{
    public class WP8Glue : GlueBase
    {
        public override void Init()
        {
            base.Init();
            Builder.RegisterModule<DefaultXCoreModule>();
            Builder.RegisterModule<DefaultWP8Module>();
            Builder.RegisterModule<XamarinGlue>();

        }
    }
}
