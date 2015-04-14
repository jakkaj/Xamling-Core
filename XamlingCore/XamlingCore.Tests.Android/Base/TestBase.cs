using Autofac;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Tests.Android.Glue;

namespace XamlingCore.Tests.Android.Base
{
    public class TestBase
    {
        protected IContainer Container;

        public TestBase()
        {
            var glue = new ProjectGlue();
            glue.Init();

            Container = glue.Container;

            ContainerHost.Container = Container;
        }

        public T Resolve<T>() where T : class
        {
            return Container.Resolve<T>();
        }
    }
}