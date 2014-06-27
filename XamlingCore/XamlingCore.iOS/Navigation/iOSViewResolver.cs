using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using XamlingCore.Xamarin.Contract;

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

            if (_scope.IsRegistered(nextToType))
            {
                var tUiView = _scope.Resolve(nextToType) as Page;
                
                if (tUiView != null)
                {
                    tUiView.BindingContext = content;
                }

                return tUiView;
            }

            throw new Exception("Content could not be resolved");
        }
    }
}