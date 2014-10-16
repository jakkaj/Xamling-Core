using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace XamlingCore.Portable.Glue.Glue
{
    /// <summary>
    /// Quickly load types from an assembly that end with the filter
    /// </summary>
    public class XAutofacModule : Module
    {
        private readonly Assembly _targetAssembly;
        private readonly string _filter;
        private readonly bool _asSelf;
        private readonly bool _singleInstance;
        private readonly bool _propertiesAutowired;

        public XAutofacModule()
        {
            
        }

        public XAutofacModule(Assembly targetAssembly, string filter, bool asSelf = false, bool singleInstance = true, bool propertiesAutowired = false)
        {
            _targetAssembly = targetAssembly;
            _filter = filter;
            _asSelf = asSelf;
            _singleInstance = singleInstance;
            _propertiesAutowired = propertiesAutowired;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_targetAssembly == null || _filter == null)
            {
                base.Load(builder);
                return;
            }

            var b = builder.RegisterAssemblyTypes(_targetAssembly)
                .Where(t => t.Name.EndsWith(_filter));

            b = _asSelf ? b.AsSelf() : b.AsImplementedInterfaces();

            if (_propertiesAutowired)
            {
                b = b.PropertiesAutowired();
            }

            if (_singleInstance)
            {
                b = b.SingleInstance();
            }

            base.Load(builder);
        }


        protected void AutoLoad(ContainerBuilder builder, Assembly targetAssembly, string filter, bool asSelf = false, bool singleInstance = false)
        {
            var b = builder.RegisterAssemblyTypes(targetAssembly)
                   .Where(t => t.Name.EndsWith(filter));

            b = asSelf ? b.AsSelf() : b.AsImplementedInterfaces();

            if (singleInstance)
            {
                b = b.SingleInstance();
            }
        }
    }
}
