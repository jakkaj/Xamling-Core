using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace XamlingCore.XamarinThings.Container
{
    public static class ContainerHost
    {
        public static ILifetimeScope Container { get; set; }
    }
}
