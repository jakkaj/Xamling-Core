using XamlingCore.iOS.Glue;
using XamlingCore.Platform.Shared.Glue;
using XamlingCore.Samples.Views.MasterDetailHome.Home;

namespace XamlingCore.Samples.iOS.Glue
{
   public  class ProjectGlue : iOSGlue
    {
       public override void Init()
       {
           base.Init(); //ensure you call this first so the builder and container are available

           //Place the type of one of your view models here, so we can find its assembly and auto register all views and view models there.
           //do this for any assemblies where you need to resolve views and view models.
           XCoreAutoRegistration.RegisterAssembly(Builder, typeof(HomeViewModel));
           
           //you can also do Builder.RegisterModule<> etc just like with Autofac - look it up :)

           Container = Builder.Build();
       }


    }
}