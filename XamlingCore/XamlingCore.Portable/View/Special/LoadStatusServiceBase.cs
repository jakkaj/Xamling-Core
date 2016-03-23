using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.Navigation;

namespace XamlingCore.Portable.View.Special
{
    public abstract class LoadStatusServiceBase : ILoadStatusService
    {
        private readonly IDispatcher _dispatcher;
        
        private readonly List<LoaderStackItem> _loaderStack = new List<LoaderStackItem>();

        public bool HideOnNavigate { get; set; }

        protected LoadStatusServiceBase(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            this.Register<NavigationMessage>(_onNavigation);
            this.Register<DisplayAlertMessage>(_onHide);

            HideOnNavigate = true;
        }

        async void _onNavigation()
        {
            if (HideOnNavigate)
            {
                await Task.Delay(500);
                _dispatcher.Invoke(HideIndicator);
            }
        }

        async void _onHide()
        {
            await Task.Delay(500);
            _dispatcher.Invoke(HideIndicator);
        }

        void _dispatch(Action method)
        {
            _dispatcher.Invoke(method);   
        }

        public async Task Loader(Task awaiter, string text = null)
        {
            var lsItem = new LoaderStackItem { Text = text };

            _loaderStack.Insert(0, lsItem);

            _dispatch(() => ShowIndicator(text));

            await awaiter;

            _updateStack(lsItem);
        }

        public async Task<T> Loader<T>(Task<T> awaiter, string text = null)
        {

            var lsItem = new LoaderStackItem { Text = text };

            _loaderStack.Insert(0, lsItem);

            _dispatch(() => ShowIndicator(text));

            await awaiter;

            _updateStack(lsItem);

            return awaiter.Result;
        }

        public abstract void ShowIndicator(string text);
        public abstract void HideIndicator();



        private void _updateStack(LoaderStackItem justFinishedItem)
        {
            lock ("loadstatus")
            {
                if (_loaderStack.Contains(justFinishedItem))
                {
                    _loaderStack.Remove(justFinishedItem);
                }

                if (_loaderStack.Count == 0)
                {
                    _dispatch(HideIndicator);

                    return;
                }
                var tItem = _loaderStack.FirstOrDefault(_ => _.Text != null);

                _dispatch(() =>
                {
                    //find the most recent item with text, and show it.

                    ShowIndicator(tItem != null ? tItem.Text : null);
                });
            }
        }

        private class LoaderStackItem
        {
            public string Text { get; set; }
        }
    }
}
