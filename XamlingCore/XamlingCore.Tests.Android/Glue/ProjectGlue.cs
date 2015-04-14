using Autofac;
using Autofac.Core;
using XamlingCore.Droid.Glue;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Tests.Android.Config;

namespace XamlingCore.Tests.Android.Glue
{

    public class ProjectGlue : DroidGlue
    {
        public override void Init()
        {
            base.Init();

            Builder.RegisterType<TestTransferConfigService>().As<IHttpTransferConfigService>().SingleInstance();

            Container = Builder.Build();


        }
    }
}