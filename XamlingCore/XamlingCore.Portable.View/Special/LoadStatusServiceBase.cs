using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;

namespace XamlingCore.Portable.View.Special
{
    public abstract class LoadStatusServiceBase : ILoadStatusService
    {
        private readonly IDispatcher _dispatcher;
        
        private readonly List<LoaderStackItem> _loaderStack = new List<LoaderStackItem>();

        protected LoadStatusServiceBase(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
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
