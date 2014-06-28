using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.iOS.Navigation
{
    public class iOSViewResolver : IViewResolver
    {
        private readonly ILifetimeScope _scope;

        public iOSViewResolver(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public Page Resolve(object content)
        {
            var t = content.GetType();

            var typeName = t.FullName.Replace("ViewModel", "View");

            //Xamarin Forms will resolve this way
            var nextToType = Type.GetType(string.Format("{0}, {1}", typeName, t.Assembly.FullName));

            var view = _resolve(nextToType);

            if (view == null)
            {
                //try doing one of it's base types
                var typeNameBase = t.BaseType.FullName.Replace("ViewModel", "View");
                var baseType = Type.GetType(string.Format("{0}, {1}", typeNameBase, t.BaseType.Assembly.FullName));
                view = _resolve(baseType);
            }

            if (view != null)
            {
                view.BindingContext = content;

                return view;
            }

            throw new Exception("Content could not be resolved");
        }

        Page _resolve(Type t)
        {
            if (t == null)
            {
                return null;
            }
            if (_scope.IsRegistered(t))
            {
                var tView = _scope.Resolve(t) as Page;

                return tView;
            }

            return null;
        }
    }
}