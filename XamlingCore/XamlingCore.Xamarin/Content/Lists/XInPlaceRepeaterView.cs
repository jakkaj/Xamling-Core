using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.XamarinThings.Content.Lists
{
    //Thanks to https://raw.githubusercontent.com/XLabs/Xamarin-Forms-Labs/master/src/Forms/XLabs.Forms/Controls/RepeaterView.cs for the inspiration

    public class XInPlaceRepeaterView<T> : ContentView
        where T : XViewModel
    {

        private IViewResolver _resolver;

        private ObservableCollection<T> _collection;

        private AbsoluteLayout _content;


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<XInPlaceRepeaterView<T>, ObservableCollection<T>>(
                p => p.ItemsSource,
                null,
                BindingMode.OneWay,
                null,
                ItemsChanged);


        public static BindableProperty ItemClickCommandProperty =
            BindableProperty.Create<XInPlaceRepeaterView<T>, ICommand>(x => x.ItemClickCommand, null);


        public XInPlaceRepeaterView()
        {
            _resolver = ContainerHost.Container.Resolve<IViewResolver>();

            _content = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            _content.SetValue(AbsoluteLayout.LayoutBoundsProperty, new Rectangle(0,0,1,1));
            _content.SetValue(AbsoluteLayout.LayoutFlagsProperty, AbsoluteLayoutFlags.All);

            Content = _content;
        }


        public ObservableCollection<T> ItemsSource
        {
            get { return (ObservableCollection<T>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }



        public ICommand ItemClickCommand
        {
            get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }



        private static void ItemsChanged(
            BindableObject bindable,
            ObservableCollection<T> oldValue,
            ObservableCollection<T> newValue)
        {
            var control = bindable as XInPlaceRepeaterView<T>;
            if (control == null)
                throw new Exception(
                    "Invalid bindable object passed to ReapterView::ItemsChanged expected a ReapterView<T> received a "
                    + bindable.GetType().Name);

            control._collectionSet(newValue);
        }

        async void _collectionSet(ObservableCollection<T> newValue)
        {
            if (_collection != null)
            {
                _collection.CollectionChanged -= _collection_CollectionChanged;
            }

            if (newValue != null)
            {
                _collection = newValue;
                _collection.CollectionChanged += _collection_CollectionChanged;

            }

            try
            {
                if (_collection != null)
                {
                    foreach (var item in _collection)
                    {
                        _addItems(item);
                        await Task.Yield();
                    }
                }
            }
            catch
            {
            }
        }

        private async void _collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                await _reset();
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<T>())
                {
                    _removeItems(item);
                    await Task.Yield();
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<T>())
                {
                    _addItems(item);
                    await Task.Yield();
                }
            }
        }

        async Task _reset()
        {

            var children = _content.Children.ToList();

            try
            {
                foreach (var child in children)
                {
                    if (_content.Children.Contains(child))
                    {
                        _content.Children.Remove(child);
                        await Task.Yield();
                    }
                }
            }
            catch
            {
            }


        }

        void _addItems(T item)
        {

            if (item == null)
            {
                return;
            }

            var view = _resolver.ResolveView(item);

            if (view == null)
            {
                return;
            }

            view.BindingContext = item;

            if (ItemClickCommand != null)
            {
                view.GestureRecognizers.Add(
                new TapGestureRecognizer { Command = ItemClickCommand, CommandParameter = item });
            }

            _content.Children.Add(view);
        }

        void _removeItems(T item)
        {
            if (item == null)
            {
                return;
            }

            var child = _content.Children.FirstOrDefault(_ => _.BindingContext == item);
            if (child != null)
            {
                _content.Children.Remove(child);
            }
        }

        protected override void OnParentSet()
        {
            Debug.WriteLine(Parent != null ? "Has parnet" : "noparent");
            base.OnParentSet();
        }
    }
}

