using Autofac;
using XamlingCore.Tests.BigWindows.Glue;

namespace XamlingCore.Tests.BigWindows.Base
{
    public class TestBase
    {
        protected IContainer Container;
        public TestBase()
        {
            var glue = new ProjectGlue();
            glue.Init();

            Container = glue.Container;
        }

        public T Resolve<T>() where T : class
        {
            return Container.Resolve<T>();
        }
    }
}