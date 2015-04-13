using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Autofac;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Tests.Droid.Glue;

namespace XamlingCore.Tests.Droid.Base
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