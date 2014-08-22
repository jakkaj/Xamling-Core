using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.Tests.iOS.Glue;
using XamlingCore.XamarinThings.Container;

namespace XamlingCore.Tests.iOS.Base
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