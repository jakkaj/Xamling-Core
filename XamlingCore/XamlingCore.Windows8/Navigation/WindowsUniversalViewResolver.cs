using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.Media;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.Windows8.Navigation
{
    public class WindowsUniversalViewResolver : IViewResolver
    {
        private readonly ILifetimeScope _scope;

        Dictionary<Type, Type> _typeCache = new Dictionary<Type,Type>();

        public WindowsUniversalViewResolver(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public Xamarin.Forms.View ResolveView(object content)
        {
            var t = content.GetType();

            Type resolveType = null;

            if (_typeCache.ContainsKey(t))
            {
                resolveType = _typeCache[t];
            }
            else
            {
                var typeName = t.FullName.Replace("ViewModel", "View");
                typeName = _filterTypeName(typeName);
                resolveType = Type.GetType(string.Format("{0}, {1}", typeName, t.GetTypeInfo().Assembly.FullName));
            }

            var cell = _resolveView(resolveType);

            if (cell != null)
            {
                if (!_typeCache.ContainsKey(t))
                {
                    _typeCache.Add(t, resolveType);
                }

                cell.BindingContext = content;
                
                return cell;
            }

            throw new Exception("Page could not be resolved for ViewModel: " + t.ToString());
        }

        public Page Resolve(object content)
        {

            var t = content.GetType();

            Type resolveType = null;

            if (_typeCache.ContainsKey(t))
            {
                resolveType = _typeCache[t];
            }
            else
            {
                var typeName = t.FullName.Replace("ViewModel", "View");
                typeName = _filterTypeName(typeName);
                resolveType = Type.GetType(string.Format("{0}, {1}", typeName, t.GetTypeInfo().Assembly.FullName));
            }
           

            var view = _resolve(resolveType);

            if (view == null)
            {
                //try doing one of it's base types
                var typeNameBase = t.GetTypeInfo().BaseType.FullName.Replace("ViewModel", "View");
                typeNameBase = _filterTypeName(typeNameBase);
                var baseType = Type.GetType(string.Format("{0}, {1}", typeNameBase, t.GetTypeInfo().BaseType.GetTypeInfo().Assembly.FullName));
                view = _resolve(baseType);
            }

            if (view != null)
            {
                if (!_typeCache.ContainsKey(t))
                {
                    _typeCache.Add(t, resolveType);
                }
               
                view.BindingContext = content;

                if (content is XViewModel && string.IsNullOrWhiteSpace(view.Title))
                {
                    view.SetBinding(Page.TitleProperty, "Title");
                }

                return view;
            }

            throw new Exception("Page could not be resolved for ViewModel: " + t.ToString());
        }

        string _filterTypeName(string typeName)
        {
            if (typeName.IndexOf("`") == -1)
            {
                return typeName;
            }
            var result = typeName.Substring(0, typeName.IndexOf("`"));
            return result;

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

        Xamarin.Forms.View _resolveView(Type t)
        {
            if (t == null)
            {
                return null;
            }

            if (_scope.IsRegistered(t))
            {
                var tView = _scope.Resolve(t) as Xamarin.Forms.View;
                return tView;
            }

            return null;
        }
    }
}