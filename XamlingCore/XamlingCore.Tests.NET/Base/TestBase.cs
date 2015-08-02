using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XamlingCore.Tests.NET.Base
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