using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace XamlingCore.Portable.Glue.TestUtils
{
    public class TestBase<T> where T : GlueBase, new()
    {
        protected IContainer Container;
        public TestBase()
        {
            var glue = new T();
            glue.Init();

            Container = glue.Container;
        }

        public T Resolve<T>() where T : class
        {
            return Container.Resolve<T>();
        }
    }
}
