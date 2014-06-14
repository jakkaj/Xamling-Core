using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Contract.Glue;

namespace XamlingCore.Portable.Glue
{
    public abstract class GlueBase : IGlue
    {
        protected ContainerBuilder Builder;

        public virtual void Init()
        {
            Builder = new ContainerBuilder();
        }

        public Autofac.IContainer Container { get; protected set; }
    }
}
