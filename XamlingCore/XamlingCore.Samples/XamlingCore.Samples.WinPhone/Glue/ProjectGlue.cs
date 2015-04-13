using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Platform.Shared.Glue;
using XamlingCore.Samples.Views.MasterDetailHome.Home;
using XamlingCore.WP8.Glue;

namespace XamlingCore.Samples.WinPhone.Glue
{
    public class ProjectGlue : WP8Glue
    {
        public override void Init()
        {
            base.Init(); //ensure you call this first so the builder and container are available

            //Place the type of one of your view models here, so we can find its assembly and auto register all views and view models there.
            //do this for any assemblies where you need to resolve views and view models.
            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(HomeViewModel));
            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(ProjectGlue));

            //you can also do Builder.RegisterModule<> etc just like with Autofac - look it up :)

            Container = Builder.Build();
        }
    }
}
